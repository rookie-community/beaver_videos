using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Areas.Workbenchs.Controllers
{
    [Area("Workbench")]
    [Authorize]
    public class DashboardController : AbpController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
