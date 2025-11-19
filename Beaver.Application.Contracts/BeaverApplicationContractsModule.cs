using Volo.Abp.Application;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule)
    )]
    public class BeaverApplicationContractsModule : AbpModule
    {

    }
}
