using Microsoft.AspNetCore.Mvc;
using MoviesLibrary.Enums;
using MoviesLibrary.Model;
using MoviesLibrary.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace BeaverVideos.Controllers
{
    public class MovieController : BaseController
    {
        private const string AnalysisBaseUrl = "https://jx.bozrc.com:4433/player/?url=";
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [Public]
        [ActionDescription("首页")]
        public IActionResult Index()
        {
            List<TopType> types = new()
            {
                TopType.Film,
                TopType.Teleplay,
                TopType.Variety,
                TopType.Anime
            };
            var tops = _movieService.GetTops(types)
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
            else if (Regex.IsMatch(name, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"))
            {
                return Redirect($"{AnalysisBaseUrl}{name}");
                //return RedirectToAction("Analysis", new { url = name });
            }
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
            int total = 0;//总数量
            ViewBag.ThisIndex = index;
            MovieDetail detail = new();
            do
            {
                if (IsFirst)
                {
                    if (linkType == 0 || catType == CatType.Variety)
                    {
                        detail = _movieService.GetDetail(catType, entId);
                        linkType = detail.ThisPlayLink;
                        if (detail.Cat == CatType.Variety)
                        {
                            break;
                        }
                    }
                    else
                    {
                        detail = _movieService.GetDetail(catType, entId, linkType);
                    }
                    limit = detail.UpInfo > 100 ? 100 : detail.PlayLinksDetail.Count;//设置每页数量
                    IsFirst = false;
                    if (detail.Cat != CatType.Film && detail.Cat != CatType.Variety)
                    {
                        total = detail.PlayLinksDetail.Max(x => x.Key);
                        detail.UpInfo = total;
                    }
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
                        lists.ToList().ForEach((item) =>
                        {
                            try
                            {
                                detail.PlayLinksDetail.Add(item.Key, item.Value);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        });
                    }
                    else if (!lists.Any() && page != 1)
                    {
                        break;
                    }
                    page++;
                }
            }
            while (detail.PlayLinksDetail.Count < total);
            detail.MovieRecommends = _movieService.GetRecommends(catType, 12, detail.Moviecategory.FirstOrDefault());
            detail.UpInfo = detail.PlayLinksDetail.Count;
            ViewBag.PlayTypeList = _movieService.GetEnumList<PlayLinkType>();
            if (detail.PlayLinksDetail.TryGetValue(index, out string url))
            {
                ViewBag.PlayUrl = $"{AnalysisBaseUrl}{url.Split("?").First()}";
            }
            else
            {
                ViewBag.PlayUrl = $"{AnalysisBaseUrl}{detail.PlayLinksDetail.FirstOrDefault().Value.Split("?").First()}";
            }
            return View(detail);
        }
    }
}
