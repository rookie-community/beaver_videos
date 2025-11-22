using Beaver.Books;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Beaver.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ConnectionStringName("Default")]
    public sealed class BeaverDbContext : AbpDbContext<BeaverDbContext>, IIdentityDbContext
    {
        #region Entities from the modules

        public DbSet<IdentityUser> Users => Set<IdentityUser>();

        public DbSet<IdentityRole> Roles => Set<IdentityRole>();

        public DbSet<IdentityClaimType> ClaimTypes => Set<IdentityClaimType>();

        public DbSet<OrganizationUnit> OrganizationUnits => Set<OrganizationUnit>();

        public DbSet<IdentitySecurityLog> SecurityLogs => Set<IdentitySecurityLog>();

        public DbSet<IdentityLinkUser> LinkUsers => Set<IdentityLinkUser>();

        public DbSet<IdentityUserDelegation> UserDelegations => Set<IdentityUserDelegation>();

        public DbSet<IdentitySession> Sessions => Set<IdentitySession>();

        #endregion

        public DbSet<Book> Books => Set<Book>();

        public BeaverDbContext(DbContextOptions<BeaverDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // 配置 Identity 模块
            builder.ConfigurePermissionManagement();
            builder.ConfigureIdentity();

            builder.Entity<Book>(b =>
            {
                b.ToTable(BeaverConsts.DbTablePrefix + "Books", BeaverConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });
        }
    }
}
