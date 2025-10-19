using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace BeaverVideos.Data
{
    public sealed class AppDbContext : AbpDbContext<AppDbContext>
    {
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置 Identity 模块
            modelBuilder.ConfigureIdentity();
        }
    }

    public class DesignTimeDbcontextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>(); optionsBuilder.UseSqlite("Data Source=BeaverVideos.db;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
