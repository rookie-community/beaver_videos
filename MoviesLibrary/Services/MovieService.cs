using System.ComponentModel;
using MoviesLibrary.Model;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Nodes;
using Polly;
using MoviesLibrary.Enums;
using System.Diagnostics;

namespace MoviesLibrary.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private readonly MemoryCacheService _memoryCacheService;
        private readonly bool _cacheState;

        /// <summary>
        /// 影视服务类
        /// </summary>
        /// <param name="memoryCacheState">是否启用缓存</param>
        /// <param name="absoluteExpiration">缓存过期时间，默认一小时过期</param>
        public MovieService(bool memoryCacheState = true, DateTimeOffset absoluteExpiration = default)
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true,
                SslProtocols = SslProtocols.Tls12,
            });
            _memoryCacheService = new MemoryCacheService(absoluteExpiration);
            _cacheState = memoryCacheState;
        }

        public IEnumerable<TopInfoModel> GetTops(TopType topType)
        {
            string JsonStr = GetHttpString(new Uri($"https://api.web.360kan.com/v1/rank?cat={(int)topType}&callback=data"));
            var obj = JsonNode.Parse(JsonStr)!["data"] as JsonArray;
            foreach (var item in obj!)
            {
                yield return new TopInfoModel
                {
                    Title = item!["title"]!.GetValue<string>(),
                    Comment = item!["comment"]!.GetValue<string>(),
                    UpInfo = item["upinfo"]!.GetValue<string>(),
                    Cat = (CatType)item["cat"]!.GetValue<int>(),
                    EntId = item["ent_id"]!.GetValue<string>(),
                    Cover = new Uri(item["cover"]!.GetValue<string>()),
                    Description = item["description"]!.GetValue<string>(),
                    Moviecat = GetListStringByJsonArray((JsonArray)item["moviecat"]!),
                    PubDate = DateTime.TryParse(item["pubdate"]!.GetValue<string>(), out DateTime date) ? date : DateTime.Now,
                    Vip = item["vip"]!.GetValue<bool>(),
                    PlayUrl = new Uri(item["url"]!.GetValue<string>()),
                    PV = item["pv"]!.GetValue<string>()
                };
            }
        }

        /// <summary>
        /// 影视搜索
        /// </summary>
        /// <param name="name">影视名称</param>
        /// <returns>ValueTuple<IEnumerable<Movie>, IEnumerable<TopModel>></returns>
        public ValueTuple<IEnumerable<Movie>, IEnumerable<TopModel>> Search(string name)
        {
            string key1 = $"Search_{name}_Movies", key2 = $"Search_{name}_Tops";
            ValueTuple<IEnumerable<Movie>, IEnumerable<TopModel>> result;
            if (_cacheState)
            {
                result.Item1 = _memoryCacheService.Get<List<Movie>>(key1);
                result.Item2 = _memoryCacheService.Get<List<TopInfoModel>>(key2);
                if (result.Item1.Any() && result.Item2.Any())
                {
                    return result;
                }
            }
            result = GetMovieAndTopsByName(name);
            if (_cacheState)
            {
                _memoryCacheService.Set(key1, result.Item1.ToList());
                _memoryCacheService.Set(key2, result.Item2.ToList());
            }
            return result;
        }

        private ValueTuple<IEnumerable<Movie>, IEnumerable<TopModel>> GetMovieAndTopsByName(string name)
        {
            List<TopModel> tops = new List<TopModel>();
            List<Movie> movies = new List<Movie>();
            try
            {
                var result = GetHttpString(new Uri($"https://api.so.360kan.com/index?force_v=1&kw={name}&from=&pageno=1&v_ap=1&tab=all&cb=data"));
                JsonNode obj = JsonNode.Parse(result)!;
                var lists = obj["data"]?["longData"]?["rows"] as JsonArray;
                var topJobj = obj["data"]?["toplist"] as JsonArray;
                foreach (var item in topJobj!)
                {
                    _ = Enum.TryParse(item!["name"]!.GetValue<string>(), out CatType cat);
                    var topItems = item["list"] as JsonArray;
                    foreach (var model in topItems!)
                    {
                        tops.Add(new TopModel
                        {
                            Title = model!["title"]!.GetValue<string>(),
                            Cat = cat,
                            Cover = new Uri(model["cover"]!.GetValue<string>()),
                            PlayUrl = new Uri(model["url"]!.GetValue<string>()),
                            PV = model["pv"]!.GetValue<string>(),
                        });
                    }
                }
                foreach (var item in lists!)
                {
                    _ = double.TryParse(item!["score"]!.GetValue<string>(), out double score);
                    _ = Enum.TryParse(item["cat_id"]!.GetValue<string>(), out CatType cat);
                    movies.Add(new Movie
                    {
                        Id = int.Parse(item["id"]!.GetValue<string>()),
                        EntId = item["en_id"]!.GetValue<string>(),
                        Cat = cat,
                        CatName = item["cat_name"]!.GetValue<string>(),
                        Cover = new Uri(item["cover"]!.GetValue<string>()),
                        CoverInfo = item["coverInfo"].Deserialize<Dictionary<string, string>>(),
                        Title = item["titleTxt"]!.GetValue<string>(),
                        Year = int.Parse(item["year"]!.GetValue<string>()),
                        Description = item["description"]!.GetValue<string>()?.Trim(),
                        Area = GetListStringByJsonArray((JsonArray)item["area"]!),
                        Tag = GetListStringByJsonArray((JsonArray)item["tag"]!),
                        Score = score,
                        ActList = GetListStringByJsonArray((JsonArray)item["actList"]!),
                        DirList = GetListStringByJsonArray((JsonArray)item["dirList"]!),
                        Vip = Convert.ToBoolean(item["vip"]!.GetValue<int>()),
                        VideoStatus = item["video_status"]!.GetValue<string>(),
                        PlayLinks = item["playlinks"].Deserialize<Dictionary<string, object>>()
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return (movies, tops);
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="cat">类型</param>
        /// <param name="EntId">编号,例如：faXpYRH6Rnb4UR</param>
        /// <param name="total">精彩推荐数量</param>
        /// <returns>MovieDetail</returns>
        public MovieDetail GetDetail(CatType cat, string EntId, int total = 12)
        {
            string key = $"Detail_{cat}_{EntId}";
            if (_cacheState)
            {
                var data = _memoryCacheService.Get<MovieDetail>(key);
                if (data.EntId == EntId)
                {
                    return data;
                }
            }
            var json = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)cat}&id={EntId}&callback=data"));
            var obj = JsonNode.Parse(json)!["data"]!;
            var result = Analysis(cat, obj, total);
            if (_cacheState)
            {
                _memoryCacheService.Set(key, result);
            }
            return result;
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="Cat">类型</param>
        /// <param name="EntId">编号,例如：faXpYRH6Rnb4UR</param>
        /// <param name="StartPage">起始页</param>
        /// <param name="EndPage">结束页</param>
        /// <param name="site">线路</param>
        /// <param name="total">精彩推荐数量</param>
        /// <returns>MovieDetail</returns>
        public MovieDetail GetDetail(CatType Cat, string EntId, int StartPage, int EndPage, PlayLinkType site, int total = 12)
        {
            if ((EndPage <= StartPage) || (EndPage - StartPage + 1 > 200))
            {
                return new MovieDetail();//页码异常
            }
            else if (Cat == CatType.Film || Cat == CatType.Variety)
            {
                return GetDetail(Cat, EntId, total);
            }
            else
            {
                string key = $"Detail_{Cat}_{EntId}_{StartPage}_{EndPage}_{site}";
                if (_cacheState)
                {
                    var data = _memoryCacheService.Get<MovieDetail>(key);
                    if (data.EntId == EntId)
                    {
                        return data;
                    }
                }
                var json = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)Cat}&id={EntId}&start={StartPage}&end={EndPage}&site={site}&callback=data"));
                var obj = JsonNode.Parse(json)!["data"]!;
                var result = Analysis(Cat, obj, total);
                if (_cacheState)
                {
                    _memoryCacheService.Set(key, result);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取推荐列表数据
        /// </summary>
        /// <param name="catType"></param>
        /// <param name="size">数量</param>
        /// <param name="CatName"></param>
        /// <returns></returns>
        public IEnumerable<MovieRecommend> GetRecommends(CatType catType, int size = 12, string? CatName = null)
        {
            string url = $"https://api.web.360kan.com/v1/filter/list?catid={(int)catType}&size={size}";
            if (!string.IsNullOrWhiteSpace(CatName))
            {
                url += $"&cat={CatName}";
            }
            url += "&callback=data";
            var JsonStr = GetHttpString(new Uri(url));
            var obj = JsonNode.Parse(JsonStr)!["data"]!["movies"] as JsonArray;
            foreach (var item in obj!)
            {
                int total = 0, upinfo = 0;
                if (catType == CatType.Anime || catType == CatType.Teleplay)
                {
                    total = item!["total"]!.GetValue<int>();
                    upinfo = item["upinfo"]!.GetValue<int>();
                }
                yield return new MovieRecommend
                {
                    Title = item!["title"]!.GetValue<string>(),
                    Comment = item["comment"]?.GetValue<string>(),
                    Cat = catType,
                    EntId = item["id"]!.GetValue<string>(),
                    Cover = new Uri($"http:{item["cdncover"]!.GetValue<string>()}"),
                    CdnCover = new Uri(item["cdncover"]!.GetValue<string>()),
                    Total = total,
                    UpInfo = upinfo,
                    Vip = item["payment"]!.GetValue<bool>()
                };
            }
        }

        /// <summary>
        /// 根据影视类型解析数据
        /// </summary>
        /// <param name="cat">影视类型</param>
        /// <param name="obj">json对象</param>
        /// <param name="total">精彩推荐数量</param>
        /// <returns></returns>
        private MovieDetail Analysis(CatType cat, JsonNode obj, int total)
        {
            if (obj != null)
            {
                var data = new MovieDetail
                {
                    Id = int.Parse(obj["id"]!.GetValue<string>()),
                    Cat = cat,
                    EntId = obj["ent_id"]!.GetValue<string>(),
                    Description = obj["description"]?.GetValue<string>(),
                    Title = obj["title"]!.GetValue<string>(),
                    Moviecategory = GetListStringByJsonArray((JsonArray)obj["moviecategory"]!),
                    Director = GetListStringByJsonArray((JsonArray)obj["director"]!),
                    PubDate = DateTime.Parse(obj["pubdate"]!.GetValue<string>()),
                    Area = GetListStringByJsonArray((JsonArray)obj["area"]!),
                    Actor = GetListStringByJsonArray((JsonArray)obj["actor"]!),
                    CdnCover = new Uri(obj["cdncover"]!.GetValue<string>()),
                    Vip = obj["vip"]!.GetValue<bool>()
                };
                if (cat == CatType.Film || cat == CatType.Variety)
                {
                    _ = double.TryParse(obj["doubanscore"]?.GetValue<string>(), out double score);
                    data.DouBanScore = score;
                    data.PlayLinkSites = GetListStringByJsonArray((JsonArray)obj["playlink_sites"]!);
                    var PlayResult = obj["playlinksdetail"].Deserialize<Dictionary<string, JsonNode>>()!.First();
                    data.ThisPlayLink = Enum.Parse<PlayLinkType>(PlayResult.Key);
                    var playLinks = PlayResult.Value;
                    data.PlayLinksDetail = new Dictionary<string, string>()
                    {
                        { playLinks["api_video_id"]!.GetValue<string>(), playLinks["default_url"]!.GetValue<string>() }
                    };
                }
                else
                {
                    data.UpInfo = obj["upinfo"]!.GetValue<int>();
                    data.Total = obj["total"]!.GetValue<int>();
                    data.PlayLinkSites = JsonSerializer.Deserialize<Dictionary<string, string>>(obj["playlinks"]!.ToJsonString())?.Select(x => x.Key).ToList();
                    var PlayResult = obj["allepidetail"].Deserialize<Dictionary<string, JsonArray>>()!.First();
                    data.ThisPlayLink = Enum.Parse<PlayLinkType>(PlayResult.Key);
                    var playLinks = PlayResult.Value;
                    data.PlayLinksDetail = playLinks!.ToDictionary(x => x!["playlink_num"]!.GetValue<string>(), x => x!["url"]!.GetValue<string>().Split("?").First());
                }
                data.MovieRecommends = GetRecommends(cat, total, data.Moviecategory.FirstOrDefault());
                return data;
            }
            else
            {
                return new MovieDetail();
            }
        }

        /// <summary>
        /// 格式化Http请求数据
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string GetHttpString(Uri uri)
        {
            return _httpClient.GetStringAsync(uri.ToString()).Result.Trim()[5..^2];
        }

        /// <summary>
        /// JsonArray转List<string>
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        private List<string> GetListStringByJsonArray(JsonArray? jsonArray)
        {
            if (jsonArray != null)
            {
                return jsonArray.Select(x => x!.GetValue<string>()).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 获取枚举类型和注解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<string, string> GetEnumList<T>() where T : Enum
        {
            Dictionary<string, string> pairs = new();
            Array values = Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                var Code = ((int)value).ToString();
                var Name = ToDescriptionString((T)value);
                pairs.Add(Code, Name);
            }
            return pairs;
        }

        private static string ToDescriptionString(Enum obj)
        {
            var attribs = (DescriptionAttribute[])obj.GetType().GetField(obj.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Length > 0 ? attribs[0].Description : obj.ToString();
        }
    }
}
