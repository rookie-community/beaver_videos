using Microsoft.AspNetCore.Mvc;
using MoviesLibrary.Common.Enum;
using MoviesLibrary.Model;
using MoviesLibrary.Services;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace BeaverVideos.Controllers
{
    public class MovieController : BaseController
    {
        private readonly MovieService _movieService;
        private readonly HttpClient _httpClient;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
            _httpClient = new HttpClient();
        }

        [Public]
        public IActionResult Index()
        {
            var tops = _movieService.GetTops();
            return View(tops);
        }

        [Public]
        public IActionResult Search(string name)
        {
            if (Regex.IsMatch(name, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"))
            {
                return RedirectToAction("Analysis", new { url = name });
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = _movieService.Search(name);
                return View(result);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Public]
        public IActionResult Analysis(string url)
        {
            //string html = await _httpClient.GetStringAsync($"https://jx.parwix.com:4433/player/analysis.php?v={url}");
            //html = html.Replace("Parwix解析系统", "小狸影视解析");
            //https://v.qq.com/x/cover/m441e3rjq9kwpsc/m00253deqqo.html
            string playlink = $"https://www.baidu.com";
            return PartialView(playlink);
        }

        [Public]
        public IActionResult Detail(string entId, CatType catType, PlayLinkType linkType, int start = 1, int end = 10)
        {
            MovieDetail detail;
            if (catType == CatType.电影 || catType == CatType.综艺)
            {
                detail = _movieService.GetDetail(catType, entId);
            }
            else
            {
                detail = _movieService.GetDetail(catType, entId, start, end, linkType);
            }
            return View(detail);
        }
    }
}
