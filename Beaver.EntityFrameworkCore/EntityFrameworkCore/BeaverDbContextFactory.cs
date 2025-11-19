using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Beaver.EntityFrameworkCore
{
    public class BeaverDbContextFactory : IDesignTimeDbContextFactory<BeaverDbContext>
    {
        public BeaverDbContext CreateDbContext(string[] args)
        {
            BeaverEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BeaverDbContext>()
                .UseSqlite(configuration.GetConnectionString("Default"));

            return new BeaverDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Acme.BookStore.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
