using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
