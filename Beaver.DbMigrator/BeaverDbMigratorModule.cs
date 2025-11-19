using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BeaverEntityFrameworkCoreModule),
        typeof(BeaverApplicationContractsModule)
    )]
    public class BeaverDbMigratorModule : AbpModule
    {

    }
}
