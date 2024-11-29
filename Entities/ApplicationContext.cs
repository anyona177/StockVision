using System.Diagnostics;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace StockVision.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        // Здесь мы говорим нашему приложению как подключиться к бд
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Проверяем, был ли уже настроен контекст
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=saaaw;Username=postgres;Password=postgres",
                    options => options
                        .MigrationsAssembly("Migrations")  // Указываем сборку миграций
                        .MigrationsHistoryTable("_EFMigrationsHistory", "public") // Таблица миграций в схеме public
                        .CommandTimeout(1000));  // Опционально, для задания тайм-аута
            }

            // Остальные настройки
            optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                          .EnableSensitiveDataLogging();
        }


        // Тут мы рассказываем компьютеру, как именно должны выглядеть таблицы в базе
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Устанавливаем схему по умолчанию для всех таблиц
            modelBuilder.HasDefaultSchema("StockVision");

            // Уникальные индексы и связи
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.Symbol)
                .IsUnique();

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.User)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Portfolio)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Asset)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HistoricalData>()
                .HasOne(h => h.Asset)
                .WithMany(a => a.HistoricalData)
                .HasForeignKey(h => h.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<HistoricalData> HistoricalData { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
    }
}
