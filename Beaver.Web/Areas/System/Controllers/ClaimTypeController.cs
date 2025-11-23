using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 声明类型
    /// </summary>
    [Area("System")]
    [Authorize]
    public class ClaimTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
