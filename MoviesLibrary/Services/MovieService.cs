using System.ComponentModel;
using MoviesLibrary.Model;
using MoviesLibrary.Common.Enum;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Nodes;
using Polly;

namespace MoviesLibrary.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private readonly MemoryCacheService _memoryCacheService;
        private readonly bool _cacheState;
        private readonly string TopsKey = "638a5c9aaeb94e2d94fc810f7e3b4073";

        /// <summary>
        /// 影视服务类
        /// </summary>
        /// <param name="memoryCacheState">是否启用缓存</param>
        /// <param name="absoluteExpiration">缓存过期时间，默认一小时过期</param>
        public MovieService(bool memoryCacheState = false, DateTimeOffset absoluteExpiration = default)
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true,
                SslProtocols = SslProtocols.Tls12,
            });
            _memoryCacheService = new MemoryCacheService(absoluteExpiration == default ? DateTimeOffset.Now.AddHours(1) : absoluteExpiration);
            _cacheState = memoryCacheState;
        }

        /// <summary>
        /// 获取排行榜数据
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IEnumerable<TopModel> GetTops()
        {
            return GetTops(x => true).ToList();
        }

        /// <summary>
        /// 获取排行榜数据
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IEnumerable<TopModel> GetTops(Func<TopModel, bool> lambda)
        {
            var tops = _memoryCacheService.Get<List<TopModel>>(TopsKey);
            if (!tops.Any())
            {
                tops = Policy.HandleResult<List<TopModel>>(x => x.Any()).Retry(3).Execute(() =>
                {
                    _ = Search("寻梦环游记").ToList();//执行一次查询，不然没数据
                    return _memoryCacheService.Get<List<TopModel>>(TopsKey);
                });
            }
            return tops.Where(lambda).ToList();
        }

        /// <summary>
        /// 影视搜索
        /// </summary>
        /// <param name="name">影视名称</param>
        /// <returns></returns>
        public IEnumerable<Movie> Search(string name)
        {
            string key = $"Search_{name}";
            IEnumerable<Movie> result;
            if (_cacheState)
            {
                result = _memoryCacheService.Get<List<Movie>>(key);
                if (result.Any())
                {
                    return result;
                }
            }
            result = GetMovieByName(name);
            if (_cacheState)
            {
                _memoryCacheService.Set(key, result.ToList());
            }
            return result;
        }

        private IEnumerable<Movie> GetMovieByName(string name)
        {
            var result = GetHttpString(new Uri($"https://api.so.360kan.com/index?force_v=1&kw={name}&from=&pageno=1&v_ap=1&tab=all&cb=data"));
            JsonNode obj = JsonNode.Parse(result)!;
            var lists = obj["data"]?["longData"]?["rows"] as JsonArray;
            if (!_memoryCacheService.Get<List<TopModel>>(TopsKey).Any())
            {
                var tops = new List<TopModel>();
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
                            Pv = model["pv"]!.GetValue<string>(),
                        });
                    }
                }
                _memoryCacheService.Set(TopsKey, tops);
            }
            foreach (var item in lists!)
            {
                _ = double.TryParse(item!["score"]!.GetValue<string>(), out double score);
                _ = int.TryParse(item["cat_id"]!.GetValue<string>(), out int cat);
                yield return new Movie
                {
                    Id = int.Parse(item["id"]!.GetValue<string>()),
                    EnId = item["en_id"]!.GetValue<string>(),
                    CatId = cat,
                    CatName = item["cat_name"]!.GetValue<string>(),
                    CoverUrl = new Uri(item["cover"]!.GetValue<string>()),
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
                };
            }
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="cat">类型</param>
        /// <param name="EntId">编号,例如：faXpYRH6Rnb4UR</param>
        /// <returns></returns>
        public MovieDetail GetDetail(CatType cat, string EntId)
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
            var result = Analysis(cat, obj);
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
        /// <returns></returns>
        public MovieDetail GetDetail(CatType Cat, string EntId, int StartPage, int EndPage, PlayLinkType site)
        {
            if (Cat == CatType.电影 || Cat == CatType.综艺)
            {
                return GetDetail(Cat, EntId);
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
                var result = Analysis(Cat, obj);
                if (_cacheState)
                {
                    _memoryCacheService.Set(key, result);
                }
                return result;
            }
        }

        /// <summary>
        /// 根据影视类型解析数据
        /// </summary>
        /// <param name="cat">影视类型</param>
        /// <param name="obj">json对象</param>
        /// <returns></returns>
        private static MovieDetail Analysis(CatType cat, JsonNode obj)
        {
            var data = new MovieDetail
            {
                Id = int.Parse(obj["id"]!.GetValue<string>()),
                EntId = obj["ent_id"]?.GetValue<string>(),
                Description = obj["description"]?.GetValue<string>(),
                Title = obj["title"]?.GetValue<string>(),
                Moviecategory = GetListStringByJsonArray((JsonArray)obj["moviecategory"]!),
                Director = GetListStringByJsonArray((JsonArray)obj["director"]!),
                PubDate = DateTime.Parse(obj["pubdate"]!.GetValue<string>()),
                Area = GetListStringByJsonArray((JsonArray)obj["area"]!),
                Actor = GetListStringByJsonArray((JsonArray)obj["actor"]!),
                CdnCover = new Uri(obj["cdncover"]!.GetValue<string>()),
                Vip = obj["vip"]!.GetValue<bool>()
            };
            if (cat == CatType.电影)
            {
                _ = double.TryParse(obj["doubanscore"]!.GetValue<string>(), out double score);
                data.DouBanScore = score;
                data.PlayLinkSites = GetListStringByJsonArray((JsonArray)obj["playlink_sites"]!);
                var playLinks = obj["playlinksdetail"].Deserialize<Dictionary<string, JsonNode>>();
                data.PlayLinksDetail = playLinks?.ToDictionary(x => x.Key, info => playLinks!.Values.ToDictionary(x => x["api_video_id"]!.GetValue<string>(), x => x["default_url"]!.GetValue<string>()));
            }
            else
            {
                data.UpInfo = obj["upinfo"]!.GetValue<int>();
                data.Total = obj["total"]!.GetValue<int>();
                data.PlayLinkSites = JsonSerializer.Deserialize<Dictionary<string, string>>(obj["playlinks"]!.ToJsonString())?.Select(x => x.Key).ToList();
                var playLinks = obj["allepidetail"].Deserialize<Dictionary<string, JsonArray>>();
                data.PlayLinksDetail = playLinks?.ToDictionary(x => x.Key, x => x.Value.ToDictionary(x => x!["id"]!.GetValue<string>(), x => x!["url"]!.GetValue<string>().Split("?").First()));
            }
            return data;
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
        private static List<string> GetListStringByJsonArray(JsonArray jsonArray)
        {
            return jsonArray.Select(x => x!.GetValue<string>()).ToList();
        }

        /// <summary>
        /// 获取枚举类型和注解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumList<T>() where T : Enum
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
