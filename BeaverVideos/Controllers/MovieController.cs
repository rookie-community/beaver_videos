using Microsoft.AspNetCore.Mvc;
using MoviesLibrary.Enums;
using MoviesLibrary.Model;
using MoviesLibrary.Services;
using System.Collections.Generic;
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
            IEnumerable<IGrouping<CatType, TopInfoModel>> tops = _movieService.GetTops(TopType.Default).DistinctBy(x => x.EntId).GroupBy(x => x.Cat).OrderBy(x => x.Key).AsEnumerable();
            return View(tops);
        }

        [Public]
        public IActionResult Search(string name)
        {
            ViewBag.Name = name;
            if (Regex.IsMatch(name, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"))
            {
                return RedirectToAction("Analysis", new { url = name });
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = _movieService.Search(name);
                return View(result.Item1);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Public]
        public async Task<IActionResult> Analysis(string url)
        {
            string html = await _httpClient.GetStringAsync($"https://okjx.cc/?url={url}");
            ViewBag.AnalysisHtml = html.Replace("OK解析", "小狸影视");
            return PartialView();
        }

        [Public]
        public IActionResult Detail(string entId, CatType catType, PlayLinkType linkType, int start = 1, int end = 10)
        {
            MovieDetail detail;
            if (catType == CatType.Film || catType == CatType.Variety)
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
