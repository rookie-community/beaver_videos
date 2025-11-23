using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [Area("System")]
    [Authorize]
    public class OrganizationUnitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
