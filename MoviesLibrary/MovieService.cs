using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MoviesLibrary.Model;
using MoviesLibrary.Common.Enum;
using System.Security.Authentication;
using System.Collections.Concurrent;

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
                    this.Search("斗罗大陆");
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
            JObject obj = JObject.Parse(result);
            var lists = obj["data"]?["longData"]?["rows"] as JArray;
            if (!_tops.Any() || (_tops.Any() && (DateTime.Now - _tops.Max(x => x.Key) > TimeSpan.FromHours(1))))
            {
                var topData = new List<TopModel>();
                var topJobj = obj["data"]?["toplist"] as JArray;
                foreach (var item in topJobj!)
                {
                    _ = Enum.TryParse(item.Value<string>("name"), out CatType cat);
                    var topItems = item["list"] as JArray;
                    foreach (var model in topItems!)
                    {
                        topData.Add(new TopModel
                        {
                            Title = model.Value<string>("title")!,
                            Cat = cat,
                            Cover = new Uri(model.Value<string>("cover")!),
                            PlayUrl = new Uri(model.Value<string>("url")!),
                            Pv = model.Value<string>("pv")!,
                        });
                    }
                }
                _tops.TryAdd(DateTime.Now, topData);
            }
            foreach (var item in lists!)
            {
                _ = double.TryParse(item.Value<string>("score"), out double score);
                yield return new Movie
                {
                    Id = item.Value<int>("id"),
                    EnId = item.Value<string>("en_id")!,
                    CatId = item.Value<int>("cat_id"),
                    CatName = item.Value<string>("cat_name"),
                    CoverUrl = new Uri(item.Value<string>("cover") ?? string.Empty),
                    CoverInfo = item["coverInfo"]?.ToList().ToDictionary(x => ((JProperty)x).Name, x => ((JProperty)x).Value.ToString()),
                    Title = item.Value<string>("titleTxt"),
                    Year = item.Value<int>("year"),
                    Description = item.Value<string>("description")?.Trim(),
                    Area = (item["area"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                    Tag = (item["tag"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                    Score = score,
                    ActList = (item["actName"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                    DirList = (item["dirList"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                    Vip = item.Value<bool>("vip"),
                    VideoStatus = item.Value<string>("video_status"),
                    PlayLinks = item["playlinks"]?.ToList().ToDictionary(x => ((JProperty)x).Name, x => ((JProperty)x).Value.ToString())
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
            var obj = JObject.Parse(result)["data"]!;
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
            var obj = JObject.Parse(result)["data"]!;
            return Analysis(Cat, obj);
        }

        /// <summary>
        /// 根据影视类型解析数据
        /// </summary>
        /// <param name="cat">影视类型</param>
        /// <param name="obj">json对象</param>
        /// <returns></returns>
        private static MovieDetail Analysis(CatType cat, JToken obj)
        {
            var data = new MovieDetail
            {
                Id = obj.Value<int>("id"),
                EntId = obj.Value<string>("ent_id"),
                Description = obj.Value<string>("description"),
                Title = obj.Value<string>("title"),
                UpInfo = obj.Value<int>("upinfo"),
                Moviecategory = (obj["moviecategory"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                Director = (obj["director"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                PubDate = DateTime.Parse(obj.Value<string>("pubdate")!),
                Area = (obj["area"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                Actor = (obj["actor"] as JArray)?.ToObject<List<string>>()?.ToArray(),
                CdnCover = new Uri(obj.Value<string>("cdncover")!),
                Vip = obj.Value<bool>("vip")
            };
            if (cat == CatType.电影)
            {
                _ = double.TryParse(obj.Value<string>("doubanscore"), out double score);
                data.DouBanScore = score;
                data.PlayLinkSites = (obj["playlink_sites"] as JArray)?.ToArray();
                data.PlayLinksDetail = (obj["playlinksdetail"] as JArray)?.ToDictionary(x => ((JProperty)x).Name, x => x.ToDictionary(x => x.Value<string>("api_video_id")!, x => x.Value<string>("default_url")!));
            }
            else
            {
                data.Total = obj.Value<int>("total");
                data.PlayLinkSites = (obj["playlinks"] as JArray)?.ToDictionary(x => ((JProperty)x).Name, x => ((JProperty)x).Value.ToString()).Select(x => x.Key).ToArray();
                data.PlayLinksDetail = (obj["allepidetail"] as JArray)?.ToDictionary(x => ((JProperty)x).Name, x => x.ToDictionary(x => x.Value<string>("id")!, x => x.Value<string>("url")!.Split("?").First()));

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
