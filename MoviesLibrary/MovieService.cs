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

namespace MoviesLibrary
{
    public class MovieService
    {
        readonly HttpClient _httpClient;

        public MovieService()
        {
            _httpClient = new HttpClient();
        }

        public MovieService(HttpClient httpClient)
        {
            if (httpClient is null)
            {
                _httpClient = new HttpClient();
            }
            else
            {
                _httpClient = httpClient;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Movie> Search(string name)
        {
            var result = _httpClient.GetAsync($"https://api.so.360kan.com/index?force_v=1&kw={name}&from=&pageno=1&v_ap=1&tab=all&cb=data").Result;
            string resultStr = result.Content.ReadAsStringAsync().Result.Trim();
            resultStr = resultStr[5..^2];
            JObject obj = JObject.Parse(resultStr);
            var lists = obj["data"]?["longData"]?["rows"] as JArray ?? new JArray();
            foreach (var item in lists)
            {
                _ = double.TryParse(item.Value<string>("score"), out double score);
                yield return new Movie
                {
                    Id = item.Value<int>("id"),
                    EnId = item.Value<string>("en_id"),
                    CatId = item.Value<int>("cat_id"),
                    CatName = item.Value<string>("cat_name"),
                    CoverUrl = new Uri(item.Value<string>("cover") ?? string.Empty),
                    CoverInfo = item["coverInfo"]?.ToList().ToDictionary(x => ((JProperty)x).Name, x => ((JProperty)x).Value.ToString()),
                    Title = item.Value<string>("titleTxt"),
                    Year = item.Value<int>("year"),
                    Description = item.Value<string>("description")?.Trim(),
                    Area = (item["area"] as JArray ?? new JArray()).ToObject<List<string>>()?.ToArray(),
                    Tag = (item["tag"] as JArray ?? new JArray()).ToObject<List<string>>()?.ToArray(),
                    Score = score,
                    ActList = (item["actName"] as JArray ?? new JArray()).ToObject<List<string>>()?.ToArray(),
                    DirList = (item["dirList"] as JArray ?? new JArray()).ToObject<List<string>>()?.ToArray(),
                    Vip = item.Value<bool>("vip"),
                    VideoStatus = item.Value<string>("video_status"),
                    PlayLinks = item["playlinks"]?.ToList().ToDictionary(x => ((JProperty)x).Name, x => ((JProperty)x).Value.ToString())
                };
            }
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="EntId"></param>
        /// <returns></returns>
        public MovieDetail GetDetail(int cat, string EntId)
        {
            var result=_httpClient.GetStringAsync("").Result;
            return new MovieDetail();
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="Cat"></param>
        /// <param name="EntId"></param>
        /// <param name="StartPage"></param>
        /// <param name="EndPage"></param>
        /// <returns></returns>
        public MovieDetail GetDetail(int Cat, string EntId,int StartPage,int EndPage)
        {
            return new MovieDetail();
        }

        public Dictionary<string, string> GetEnumList<T>() where T : Enum
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            Array values = Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                var Code = ((int)value).ToString();
                var Name = ToDescriptionString((T)value);
                pairs.Add(Code, Name);
            }
            return pairs;
        }

        private string ToDescriptionString(Enum obj)
        {
            var attribs = (DescriptionAttribute[])obj.GetType().GetField(obj.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Length > 0 ? attribs[0].Description : obj.ToString();
        }
    }
}
