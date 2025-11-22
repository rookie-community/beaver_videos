using Beaver.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverDomainModule),
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule)
    )]
    public class BeaverEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BeaverDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                // 使用 SQLite 提供程序（或根据你的情况替换为 UseSqlServer 等）
                options.UseSqlite();
            });
        }
    }
}
