using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverDomainModule),
        typeof(BeaverApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        //typeof(AbpIdentityApplicationModule)
    )]
    public class BeaverApplicationModule : AbpModule
    {

    }
}
