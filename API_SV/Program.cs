using Microsoft.EntityFrameworkCore;
using API_SV;
using StockVision.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("StockVisionDb")));

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("http://localhost:8080") // Укажите URL фронтенда
               .AllowAnyOrigin()  // Разрешаем любые источники
               .AllowAnyMethod()  // Разрешаем любые методы (GET, POST и т. д.)
               .AllowAnyHeader());  // Разрешаем любые заголовки
});

// Настройка JWT аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Убедитесь, что "Jwt:Issuer" задан корректно
            ValidAudience = builder.Configuration["Jwt:Audience"], // Убедитесь, что "Jwt:Audience" совпадает
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) // Проверьте ключ
        };
    });

// Добавление авторизации
builder.Services.AddAuthorization();

// Регистрация контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Применение CORS (должно быть до UseAuthentication и UseAuthorization)
app.UseCors("AllowAll");

// Включение аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Регистрация маршрутов для контроллеров
app.MapControllers();

// Маршруты API
app.MapGet("/", () => "StockVision API");

app.MapUserCredentialsApi();

app.MapGroup("/users").MapUsersApi().RequireAuthorization();
app.MapGroup("/assets").MapAssetsApi().RequireAuthorization();
app.MapGroup("/portfolios").MapPortfoliosApi().RequireAuthorization();
app.MapGroup("/transactions").MapTransactionsApi().RequireAuthorization();
app.MapGroup("/historicaldata").MapHistoricalDataApi().RequireAuthorization();

// Запуск приложения
app.Run();

