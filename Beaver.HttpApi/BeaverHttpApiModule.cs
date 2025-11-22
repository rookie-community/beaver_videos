using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverApplicationContractsModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule)
    )]
    public class BeaverHttpApiModule : AbpModule
    {

    }
}
