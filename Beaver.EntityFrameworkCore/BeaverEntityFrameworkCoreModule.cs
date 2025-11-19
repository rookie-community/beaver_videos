using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverDomainModule),
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(AbpIdentityEntityFrameworkCoreModule)
    )]
    public class BeaverEntityFrameworkCoreModule : AbpModule
    {

    }
}
