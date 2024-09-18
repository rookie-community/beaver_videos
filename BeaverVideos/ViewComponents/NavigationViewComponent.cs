using Microsoft.AspNetCore.Mvc;

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
