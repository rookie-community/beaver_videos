using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeaverVideos.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string name = "")
        {
            ViewBag.Name = name;
            return View();
        }
    }
}
