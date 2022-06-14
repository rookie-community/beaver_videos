using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeaverVideos.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NavigationViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
