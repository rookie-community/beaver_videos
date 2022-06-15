using Microsoft.AspNetCore.Mvc;
using MoviesLibrary.Enums;
using MoviesLibrary.Model;
using MoviesLibrary.Services;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace BeaverVideos.Controllers
{
    public class MovieController : BaseController
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [Public]
        [ActionDescription("首页")]
        public IActionResult Index()
        {
            var tops = _movieService.GetTops(TopType.Default)
                .DistinctBy(x => x.EntId)
                .GroupBy(x => x.Cat)
                .OrderBy(x => x.Key)
                .AsEnumerable();
            return View(tops);
        }

        [Public]
        [ActionDescription("搜索页")]
        public IActionResult Search(string name)
        {
            ViewBag.Name = name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            //else if (Regex.IsMatch(name, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"))
            //{
            //    return RedirectToAction("Analysis", new { url = name });
            //}
            else
            {
                var result = _movieService.Search(name);
                return View(result.Item1);
            }
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="entId">编号</param>
        /// <param name="catType">类型</param>
        /// <param name="linkType">线路</param>
        /// <param name="index">当前选集</param>
        /// <returns></returns>
        [Public]
        [ActionDescription("详情页")]
        public IActionResult Detail(string entId, CatType catType, PlayLinkType linkType, int index = 1)
        {
            if (entId == null || catType == 0)
            {
                return Redirect("/");
            }
            bool IsFirst = true;//是否第一次请求
            int page = 1;
            int limit = 50;
            string baseUrl = "https://jx.parwix.com:4433/player/analysis.php?v=";
            ViewBag.ThisIndex = index;
            MovieDetail detail = new MovieDetail();
            do
            {
                if (IsFirst)
                {
                    detail = _movieService.GetDetail(catType, entId);
                    limit = detail.PlayLinksDetail.Count;//设置每页数量
                    if (linkType == 0)
                    {
                        linkType = detail.ThisPlayLink;
                    }
                    IsFirst = false;
                }
                else
                {
                    int start = (page - 1) * limit + 1;
                    int end = page * limit > detail.UpInfo ? detail.UpInfo : page * limit;
                    var lists = _movieService.GetDetail(catType, entId, start, end, linkType).PlayLinksDetail;
                    if (page == 1)
                    {
                        detail.PlayLinksDetail = lists;
                    }
                    else if (lists.Any())
                    {
                        foreach (var item in lists)
                        {
                            detail.PlayLinksDetail.Add(item.Key, item.Value);
                        }
                    }
                    page++;
                }
            }
            while (detail.PlayLinksDetail.Count < detail.UpInfo);
            detail.MovieRecommends = _movieService.GetRecommends(catType, 12, detail.Moviecategory.FirstOrDefault());
            if (detail.PlayLinksDetail.TryGetValue(index.ToString(), out string url))
            {
                ViewBag.PlayUrl = baseUrl + url;
            }
            else
            {
                ViewBag.PlayUrl = baseUrl + detail.PlayLinksDetail.FirstOrDefault().Value;
            }
            return View(detail);
        }
    }
}
