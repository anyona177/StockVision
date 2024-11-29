using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace StockVision.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            const string connectionString =
                "Host=localhost;Port=5434;Database=saaaw;User ID=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            using ApplicationContext db = new(
                optionsBuilder.UseNpgsql(connectionString).Options);

            // Удаление всех пользователей и данных для чистого теста
            db.Users.RemoveRange(db.Users.ToList());
            db.Portfolios.RemoveRange(db.Portfolios.ToList());
            db.Assets.RemoveRange(db.Assets.ToList());
            db.SaveChanges();

            // Создание новых пользователей
            db.Users.Add(new User
            {
                UserId = 1,
                Name = "user1",
                Email = "user1@example.com",
                Role = "Admin",
                RegistrationDate = DateTime.Now
            });

            db.Users.Add(new User
            {
                UserId = 2,
                Name = "user2",
                Email = "user2@example.com",
                Role = "Analyst",
                RegistrationDate = DateTime.Now
            });

            // Пример добавления сущностей
            db.Portfolios.Add(new Portfolio
            {
                PortfolioId = 1,
                UserId = 1,
                Name = "Portfolio 1" // Установлено обязательное поле Name
            });

            db.Assets.Add(new Asset
            {
                AssetId = 1,
                Type = "Stock",  // Установлено обязательное поле Type
                // добавьте другие обязательные поля, если они есть
            });

            db.Transactions.Add(new Transaction
            {
                TransactionId = 1,
                PortfolioId = 1,
                AssetId = 1,
                Quantity = 100,
                TransactionDate = DateTime.Now
            });

            db.SaveChanges();

            // Запрос для проверки
            var users = db.Users.ToList();
            var portfolios = db.Portfolios.ToList();

            Console.WriteLine($"Users: {users.Count}");
            Console.WriteLine($"Portfolios: {portfolios.Count}");
            Console.WriteLine("Test completed successfully.");
        }
    }
}
