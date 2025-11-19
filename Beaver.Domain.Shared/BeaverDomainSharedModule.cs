using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(AbpDddDomainSharedModule),
        typeof(AbpIdentityDomainSharedModule)
    )]
    public class BeaverDomainSharedModule : AbpModule
    {

    }
}
