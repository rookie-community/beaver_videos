using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverApplicationContractsModule)
    )]
    public class BeaverHttpApiModule : AbpModule
    {

    }
}
