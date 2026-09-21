using Beaver.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [Area("System")]
    [Authorize(Roles = AppRoles.Admin)]
    public class OrganizationUnitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
