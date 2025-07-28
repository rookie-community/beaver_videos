using BeaverVideos.Dtos.Enums;
using BeaverVideos.Dtos.Movies;
using BeaverVideos.Services.Interfaces;
using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BeaverVideos.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly Dictionary<string, string> analysisDictionary = new Dictionary<string, string>();
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IMovieService movieService, IWebHostEnvironment environment)
        {
            _movieService = movieService;
            _environment = environment;
            analysisDictionary = new Dictionary<string, string>
            {
                { "虾米解析" , "https://jx.xmflv.com/?url=" },
                { "极速云解析" , "https://jx.2s0.cn/player/?url=" },
                { "七哥解析" , "https://jx.nnxv.cn/tv.php?url=" },
                { "jy解析" , "https://media.staticfile.link/?iv=3232302e3230322e3131342e3137&key=2264f8ca9cfad6b17ff57bcf3fe4aee7&url=" },
                //{ "jy解析" , "https://jx.playerjy.com/?url=" },
                { "咸鱼解析" , "https://jx.xymp4.cc/?url=" },
            };
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var movieTypes = new List<MovieType>()
            {
                MovieType.Default,
                MovieType.Teleplay,
                MovieType.Film,
                MovieType.Variety,
                MovieType.Children,
                MovieType.Anime,
                MovieType.General,
            };
            var carouselResult = await _movieService.GetCarousel(cancellationToken: cancellationToken);

            var movies = new Dictionary<MovieType, List<MovieRecommendResponse>>();
            foreach (var movieType in movieTypes)
            {
                var itemResult = await _movieService.GetTops(movieType, cancellationToken: cancellationToken);
                movies.TryAdd(movieType, itemResult.ValueOrDefault);
            }

            var vm = new IndexViewModel
            {
                Carousels = carouselResult.ValueOrDefault,
                Movies = movies
            };
            return View(vm);
        }

        public async Task<IActionResult> Search(string name, CancellationToken cancellationToken = default)
        {
            ViewBag.Name = name;
            if (Regex.IsMatch(name, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"))
            {
                //return Redirect($"{AnalysisBaseUrl}{name}");
                return RedirectToAction(nameof(Analysis), new { url = name });
            }
            var result = await _movieService.MovieQuery(name, cancellationToken);
            return View(result.ValueOrDefault);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieName(string name, CancellationToken cancellationToken = default)
        {
            var result = await _movieService.QuerySuggestion(name, cancellationToken);
            return Ok(result.ValueOrDefault);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="entId">编号</param>
        /// <param name="catType">类型</param>
        /// <param name="linkType">线路</param>
        /// <param name="index">当前选集</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(string entId, CatType catType, PlayLinkSites linkType, int index = 1, CancellationToken cancellationToken = default)
        {
            try
            {
                var movieDetailResult = await _movieService.GetMovieDetail(catType, entId, linkType, cancellationToken: cancellationToken);
                var movieDetail = movieDetailResult.ValueOrDefault;
                if (!movieDetailResult.IsSuccess)
                {
                    return movieDetailResult.ToActionResult();
                }

                var currentPlayLink = linkType;
                if (linkType == default)
                {
                    _ = Enum.TryParse(movieDetail.PlaylinkSites.FirstOrDefault(), out currentPlayLink);
                }

                var playLinkSites = new List<PlayLinkSites>();
                foreach (var item in movieDetail.PlaylinkSites)
                {
                    if (Enum.TryParse<PlayLinkSites>(item, out var playLink))
                    {
                        playLinkSites.Add(playLink);
                    }
                }

                var tag = movieDetail.Moviecategory.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var recommendCount = 12;
                var recommend = new List<ExcitingRecommendationsDto>();
                var recommendResult = await _movieService.GetRecommend(catType, tag, recommendCount + 6, cancellationToken: cancellationToken);
                if (recommendResult.IsSuccess)
                {
                    recommend = recommendResult.Value.Where(x => x.Id != entId).OrderBy(x => Guid.NewGuid()).Take(recommendCount).ToList();
                }

                var playUrl = string.Empty;
                var playLinksDetails = new List<AllepidetailItem>();
                if (catType == CatType.Film)
                {
                    var temp = movieDetail.PlayLinksDetail[$"{currentPlayLink}"];
                    var playLinksDetailItem = new AllepidetailItem
                    {
                        Id = temp.Id,
                        ApiId = temp.ApiId,
                        ApiVideoId = temp.ApiVideoId,
                        Url = temp.DefaultUrl,
                        IsVip = $"{Convert.ToInt32(movieDetail.Vip)}"
                    };
                    playLinksDetails.Add(playLinksDetailItem);
                    playUrl = playLinksDetailItem.Url;
                }
                else
                {
                    movieDetail.Allepidetail.TryGetValue($"{currentPlayLink}", out playLinksDetails);
                    if (movieDetail.UpInfo > 0 && movieDetail.UpInfo != playLinksDetails?.Count)
                    {
                        var result2 = await _movieService.GetMovieDetail(catType, entId, currentPlayLink, movieDetail.UpInfo, cancellationToken);
                        if (!result2.IsSuccess)
                        {
                            return result2.ToActionResult();
                        }
                        playLinksDetails = result2.ValueOrDefault;
                    }

                    playUrl = playLinksDetails?.Where(x => x.PlaylinkNum==$"{index}").Select(x => x.Url).FirstOrDefault();
                }

                var vm = new MovieDetailViewModel
                {
                    Title = movieDetail.Title,
                    EntId = movieDetail.EntId,
                    Moviecategory = movieDetail.Moviecategory,
                    CurrentPlayLink = currentPlayLink,
                    PlayLinkSites = playLinkSites,
                    CatType = catType,
                    CurrentIndex = index,
                    CurrentPlayUrl = GetAnalysisUrl(playUrl!),
                    Vip = movieDetail.Vip,
                    Description = movieDetail.Description,
                    Recommends = recommend,
                    PlayLinksDetail = playLinksDetails ?? new List<AllepidetailItem>()
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message).ToActionResult();
            }
        }

        public IActionResult Analysis(string url)
        {
            var analysisUrl = GetAnalysisUrl(url);
            return View(model: $"{analysisUrl}{url}");
        }

        private string GetAnalysisUrl(string playUrl)
        {
            //if (_environment.IsDevelopment())
            //{
            //    return string.Empty;
            //}
            var analysisUrl = analysisDictionary.OrderBy(x => Guid.NewGuid()).Select(x => x.Value).FirstOrDefault();
            return $"{analysisUrl}{playUrl}";
        }
    }
}
