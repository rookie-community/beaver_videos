using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoviesLibrary.Model;
using MoviesLibrary.Common.Enum;
using System.Security.Authentication;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;
using Polly;

namespace MoviesLibrary
{
    public class MovieService
    {
        readonly HttpClient _httpClient;
        readonly ConcurrentDictionary<DateTime, List<TopModel>> _tops;//排行榜数据

        public MovieService()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true,
                SslProtocols = SslProtocols.Tls12,
            });
            _tops = new ConcurrentDictionary<DateTime, List<TopModel>>();
        }

        /// <summary>
        /// 获取排行榜数据
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IEnumerable<TopModel> GetTops(Func<TopModel, bool> lambda)
        {
            List<TopModel> topModels = new();
            try
            {
                if (!_tops.Any())
                {
                    var result = this.Search("斗罗大陆");
                    Policy.HandleResult(result.Any()).Retry(3);
                }
                var key = _tops.Max(x => x.Key);
                _tops.TryGetValue(key, out topModels!);
                return topModels.Where(lambda).ToList();
            }
            catch (Exception)
            {
                return topModels;
            }
        }

        /// <summary>
        /// 影视搜索
        /// </summary>
        /// <param name="name">影视名称</param>
        /// <returns></returns>
        public IEnumerable<Movie> Search(string name)
        {
            var result = GetHttpString(new Uri($"https://api.so.360kan.com/index?force_v=1&kw={name}&from=&pageno=1&v_ap=1&tab=all&cb=data"));
            JsonNode obj = JsonNode.Parse(result)!;
            var lists = obj["data"]?["longData"]?["rows"] as JsonArray;
            if (!_tops.Any() || (_tops.Any() && (DateTime.Now - _tops.Max(x => x.Key) > TimeSpan.FromHours(1))))
            {
                var topData = new List<TopModel>();
                var topJobj = obj["data"]?["toplist"] as JsonArray;
                foreach (var item in topJobj!)
                {
                    _ = Enum.TryParse(item!["name"]!.GetValue<string>(), out CatType cat);
                    var topItems = item["list"] as JsonArray;
                    foreach (var model in topItems!)
                    {
                        topData.Add(new TopModel
                        {
                            Title = model!["title"]!.GetValue<string>(),
                            Cat = cat,
                            Cover = new Uri(model["cover"]!.GetValue<string>()),
                            PlayUrl = new Uri(model["url"]!.GetValue<string>()),
                            Pv = model["pv"]!.GetValue<string>(),
                        });
                    }
                }
                _tops.TryAdd(DateTime.Now, topData);
            }
            foreach (var item in lists!)
            {
                _ = double.TryParse(item!["score"]!.GetValue<string>(), out double score);
                yield return new Movie
                {
                    Id = int.Parse(item["id"]!.GetValue<string>()),
                    EnId = item["en_id"]!.GetValue<string>(),
                    CatId = int.Parse(item["cat_id"]!.GetValue<string>()),
                    CatName = item["cat_name"]!.GetValue<string>(),
                    CoverUrl = new Uri(item["cover"]!.GetValue<string>()),
                    CoverInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(item["coverInfo"]),
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
                    PlayLinks = JsonSerializer.Deserialize<Dictionary<string, string>>(item["playlinks"])
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
            var result = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)cat}&id={EntId}&callback=data"));
            var obj = JsonNode.Parse(result)!["data"]!;
            return Analysis(cat, obj);
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
            var result = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)Cat}&id={EntId}&start={StartPage}&end={EndPage}&site={site}&callback=data"));
            var obj = JsonNode.Parse(result)!["data"]!;
            return Analysis(Cat, obj);
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
                var playLinks = JsonSerializer.Deserialize<Dictionary<string, JsonNode>>(obj["playlinksdetail"]);
                data.PlayLinksDetail = playLinks?.ToDictionary(x => x.Key, info => playLinks!.Values.ToDictionary(x => x["api_video_id"]!.GetValue<string>(), x => x["default_url"]!.GetValue<string>()));
            }
            else
            {
                data.UpInfo = obj["upinfo"]!.GetValue<int>();
                data.Total = obj["total"]!.GetValue<int>();
                data.PlayLinkSites = JsonSerializer.Deserialize<Dictionary<string, string>>(obj["playlinks"]!.ToJsonString())?.Select(x => x.Key).ToList();
                var playLinks = JsonSerializer.Deserialize<Dictionary<string, JsonArray>>(obj["allepidetail"]);
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
