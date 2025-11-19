using Beaver.Localization;
using Volo.Abp.Application.Services;

namespace Beaver
{
/* Inherit your application services from this class.
*/
    public abstract class BeaverAppService : ApplicationService
    {
        protected BeaverAppService()
        {
            LocalizationResource = typeof(BeaverResource);
        }
    }
}
