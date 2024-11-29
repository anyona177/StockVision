using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockVision.Entities;

namespace API_SV
{
    public static class UserCredentialsApi
    {
        public static void MapUserCredentialsApi(this WebApplication app)
        {
            app.MapPost("/register", async ([FromBody] UserRegistrationRequest request, ApplicationContext dbContext) =>
            {
                // Проверка на существующего пользователя
                if (await dbContext.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return Results.BadRequest("User with this email already exists.");
                }

                // Проверка допустимой роли
                var allowedRoles = new[] { "User", "Admin" };
                if (!allowedRoles.Contains(request.Role))
                {
                    return Results.BadRequest("Invalid role. Allowed roles are 'User' and 'Admin'.");
                }

                // Создание нового пользователя
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Role = request.Role, // Устанавливаем роль
                    RegistrationDate = DateTime.UtcNow
                };

                // Добавление пользователя в таблицу Users
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                // Хэширование пароля
                var passwordHash = HashPassword(request.Password);

                // Создание записи в таблице UserCredentials
                var credentials = new UserCredentials
                {
                    UserId = user.UserId,
                    User = user,
                    PasswordHash = passwordHash,
                    LastLogin = DateTime.MinValue
                };

                await dbContext.Set<UserCredentials>().AddAsync(credentials);
                await dbContext.SaveChangesAsync();

                return Results.Ok(new { message = "User registered successfully." });
            });

            app.MapPost("/login", async ([FromBody] UserLoginRequest request, ApplicationContext dbContext, IConfiguration config) =>
            {
                // Поиск пользователя
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                {
                    return Results.BadRequest("Invalid email or password.");
                }

                // Проверка пароля
                var credentials = await dbContext.Set<UserCredentials>()
                    .FirstOrDefaultAsync(uc => uc.UserId == user.UserId);

                if (credentials == null || credentials.PasswordHash != HashPassword(request.Password))
                {
                    return Results.BadRequest("Invalid email or password.");
                }

                // Обновление времени последнего входа
                credentials.LastLogin = DateTime.UtcNow;
                dbContext.Update(credentials);
                await dbContext.SaveChangesAsync();

                // Генерация JWT токена
                var token = GenerateJwtToken(user, config);

                // Логирование для отладки
                Console.WriteLine($"Stored hash: {credentials.PasswordHash}, Provided hash: {HashPassword(request.Password)}");

                // Возвращаем токен и дополнительные данные
                return Results.Ok(new
                {
                    Token = token,
                    Role = user.Role, // Роль пользователя
                    Name = user.Name  // Имя пользователя
                });
            });
        }

        public static string GenerateJwtToken(User user, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            // Хэширование пароля с использованием SHA512
            byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hash).Replace("-", "").ToLower(); // Преобразуем в строку в нижнем регистре
        }
    }

    public record UserRegistrationRequest(string Name, string Email, string Password, string Role);
    public record UserLoginRequest(string Email, string Password);
}
