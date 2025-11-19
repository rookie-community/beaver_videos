using Volo.Abp.Domain;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverDomainSharedModule),
        typeof(AbpDddDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpEmailingModule)
    )]
    public class BeaverDomainModule : AbpModule
    {

    }
}
