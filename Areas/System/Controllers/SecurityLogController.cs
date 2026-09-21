using Beaver.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 安全日志
    /// </summary>
    [Area("System")]
    [Authorize(Roles = AppRoles.Admin)]
    public class SecurityLogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
