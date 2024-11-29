using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StockVision.Entities;

namespace StockVision.Migrations
{
    public class MigrationsContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=saaaw;Username=postgres;Password=postgres");

            // Укажите сборку миграций
            optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=saaaw;Username=postgres;Password=postgres",
                options => options.MigrationsAssembly("Migrations"));

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
