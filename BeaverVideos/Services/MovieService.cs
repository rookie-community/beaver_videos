using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Diagnostics;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using BeaverVideos.Common.Enums;
using BeaverVideos.Dto;
using System.Threading.Tasks;

namespace BeaverVideos.Services
{
    public class MovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// 影视服务类
        /// </summary>
        public MovieService(IHttpClientFactory httpClientFactory)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true

            };
            _httpClientFactory = httpClientFactory;
        }

        #region 旧方法

        /// <summary>
        /// 获取排行榜数据
        /// </summary>
        /// <param name="topType">类型</param>
        /// <returns></returns>
        public IEnumerable<TopInfoModel> GetTops2(TopType topType)
        {
            Test();
            string JsonStr = GetHttpString(new Uri($"https://api.web.360kan.com/v1/rank?cat={(int)topType}&callback=data"));
            var obj = JsonNode.Parse(JsonStr)!["data"] as JsonArray;
            foreach (var item in obj!)
            {
                TopInfoModel top;
                try
                {
                    top = new TopInfoModel
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
                        PlayUrl = new Uri(item["url"]?.GetValue<string>() ?? ""),
                        PV = item["pv"]!.GetValue<string>()
                    };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(top.Title))
                {
                    continue;
                }
                yield return top;
            }
        }

        /// <summary>
        /// 获取排行榜数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TopInfoModel> GetTops(List<TopType> types)
        {
            List<TopInfoModel> tops = new();
            types.ForEach((item) =>
            {
                tops.AddRange(GetTops2(item));
            });
            return tops.OrderBy(x => x.Cat);
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
            result = GetMovieAndTopsByName(name);
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
        /// <param name="getRecommends">是否需要精彩推荐数据</param>
        /// <param name="total">精彩推荐数量</param>
        /// <returns>MovieDetail</returns>
        public MovieDetail GetDetail(CatType cat, string EntId, PlayLinkType linkType = 0, bool getRecommends = false, int total = 12)
        {
            string key = $"Detail_{cat}_{EntId}_{linkType}_{getRecommends}_{total}";
            var json = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)cat}&id={EntId}{(linkType == 0 ? null : $"&site={linkType}")}&callback=data"));
            var obj = JsonNode.Parse(json)!["data"]!;
            var result = Analysis(cat, obj, linkType, getRecommends, total);
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
        /// <param name="getRecommends">是否需要精彩推荐数据</param>
        /// <param name="total">精彩推荐数量</param>
        /// <returns>MovieDetail</returns>
        public MovieDetail GetDetail(CatType Cat, string EntId, int StartPage, int EndPage, PlayLinkType site, bool getRecommends = false, int total = 12)
        {
            if (EndPage <= StartPage || EndPage - StartPage + 1 > 200)
            {
                return new MovieDetail();//页码异常
            }
            else if (Cat == CatType.Film || Cat == CatType.Variety)
            {
                return GetDetail(Cat, EntId, site, getRecommends, total);
            }
            else
            {
                string key = $"Detail_{Cat}_{EntId}_{StartPage}_{EndPage}_{site}_{getRecommends}_{total}";
                var json = GetHttpString(new Uri($"https://api.web.360kan.com/v1/detail?cat={(int)Cat}&id={EntId}&start={StartPage}&end={EndPage}&site={site}&callback=data"));
                var obj = JsonNode.Parse(json)!["data"]!;
                var result = Analysis(Cat, obj, site, getRecommends, total);
                return result;
            }
        }

        /// <summary>
        /// 获取推荐列表数据
        /// </summary>
        /// <param name="catType">影视类型</param>
        /// <param name="size">数量</param>
        /// <param name="CatName">关联词</param>
        /// <returns></returns>
        public IEnumerable<MovieRecommend> GetRecommends(CatType catType, int size = 12, string CatName = null)
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
        /// <param name="getRecommends">是否获取推荐数据</param>
        /// <returns></returns>
        private MovieDetail Analysis(CatType cat, JsonNode obj, PlayLinkType site, bool getRecommends = false, int total = 12)
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
                try
                {
                    if (cat == CatType.Film || cat == CatType.Variety)
                    {
                        _ = double.TryParse(obj["doubanscore"]?.GetValue<string>(), out double score);
                        data.DouBanScore = score;
                        data.PlayLinkSites = GetListStringByJsonArray((JsonArray)obj["playlink_sites"]!);
                        var PlayResult = obj["playlinksdetail"].Deserialize<Dictionary<string, JsonNode>>();
                        var playObj = PlayResult!.First();//默认第一项
                        if (cat == CatType.Film && site != 0)
                        {
                            if (PlayResult!.ContainsKey(site.ToString()))
                            {
                                playObj = PlayResult!.First(x => x.Key.Equals(site.ToString()));
                            }
                        }
                        data.ThisPlayLink = Enum.Parse<PlayLinkType>(playObj.Key);
                        var playLinks = playObj.Value;
                        data.PlayLinksDetail = new Dictionary<int, string>()
                    {
                        {data.PlayLinksDetail.Count+1, playLinks["default_url"]!.GetValue<string>() }
                    };


                        if (cat == CatType.Variety)
                        {
                            var otherData = (JsonArray)obj["defaultepisode"]!;
                            foreach (var item in otherData)
                            {
                                if (!string.IsNullOrWhiteSpace(item!["url"]!.GetValue<string>()))
                                {
                                    data.PlayLinksDetail.TryAdd(data.PlayLinksDetail.Count + 1, item!["url"]!.GetValue<string>());
                                }
                            }
                        }
                    }
                    else
                    {
                        data.UpInfo = obj["upinfo"]!.GetValue<int>();
                        data.Total = obj["total"]!.GetValue<int>();
                        data.PlayLinkSites = JsonSerializer.Deserialize<Dictionary<string, string>>(obj["playlinks"]!.ToJsonString())?.Select(x => x.Key).ToList();
                        var PlayResult = obj["allepidetail"].Deserialize<Dictionary<string, JsonArray>>()!.First();
                        data.ThisPlayLink = Enum.Parse<PlayLinkType>(PlayResult.Key);
                        var playLinks = PlayResult.Value;
                        playLinks.ToList().ForEach((item) =>
                        {
                            if (!string.IsNullOrWhiteSpace(item!["url"]!.GetValue<string>()))
                            {
                                data.PlayLinksDetail.Add(int.Parse(item!["playlink_num"]!.GetValue<string>()), item!["url"]!.GetValue<string>());
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                if (getRecommends)
                {
                    data.MovieRecommends = GetRecommends(cat, total, data.Moviecategory.FirstOrDefault());
                }
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
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var temp = client.GetStringAsync(uri).Result.Trim()[5..^2];
                return temp;
            }
            catch (Exception)
            {
                return "404错误";
            }
        }

        /// <summary>
        /// JsonArray转List<string>
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        private List<string> GetListStringByJsonArray(JsonArray jsonArray)
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

        #endregion


        private void Test()
        {
            GetRecommend(CatType.Anime, "热血").Wait();
        }

        /// <summary>
        /// 搜索提示
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <returns></returns>
        public async Task<ResultDto<List<SuggestionDto>>> SearchQuerySuggestion(string content)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.so.360kan.com/suggest.php?kw={content}";
                var jsonResult = await client.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<ResultDto>(jsonResult, _jsonSerializerOptions);
                if (!result.IsSuccess)
                {
                    return new ResultDto<List<SuggestionDto>>
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }

                var obj = JsonNode.Parse(jsonResult)!;
                var jsonData = obj["data"]["suglist"].ToJsonString();
                var data = JsonSerializer.Deserialize<List<SuggestionDto>>(jsonData, _jsonSerializerOptions);
                return new ResultDto<List<SuggestionDto>>
                {
                    Data = data,
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                var result = new ResultDto<List<SuggestionDto>>
                {
                    Message = ex.Message,
                    Code = 500
                };
                return result;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ResultDto<List<QueryResultDto>>> Query(string name)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.so.360kan.com/index?kw={name}&pageno=1";
                var jsonResult = await client.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<ResultDto>(jsonResult, _jsonSerializerOptions);
                if (!result.IsSuccess)
                {
                    return new ResultDto<List<QueryResultDto>>
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }

                var obj = JsonNode.Parse(jsonResult)!;
                var jsonData = obj["data"]["longData"]["rows"].ToJsonString();

                var data = JsonSerializer.Deserialize<List<QueryResultDto>>(jsonData, _jsonSerializerOptions);
                return new ResultDto<List<QueryResultDto>>
                {
                    Data = data,
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                var result = new ResultDto<List<QueryResultDto>>
                {
                    Message = ex.Message,
                    Code = 500
                };
                return result;
            }
        }

        /// <summary>
        /// 影视推荐
        /// </summary>
        /// <param name="cat">类型</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        public async Task<ResultDto<List<RecommendDto>>> GetRecommend(CatType cat, int size = 8)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var jsonResult = await client.GetStringAsync($"https://api.web.360kan.com/v1/rank?cat={(int)cat}&size={size}");
                var result = JsonSerializer.Deserialize<ResultDto<List<RecommendDto>>>(jsonResult, _jsonSerializerOptions);
                if (!result.IsSuccess)
                {
                    return new ResultDto<List<RecommendDto>>
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new ResultDto<List<RecommendDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                };
                return result;
            }
        }

        /// <summary>
        /// 精彩推荐
        /// </summary>
        /// <param name="cat">类型</param>
        /// <param name="tag">标签，例如：冒险</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        public async Task<ResultDto<List<ExcitingRecommendationsDto>>> GetRecommend(CatType cat, string tag, int size = 18)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var jsonResult = await client.GetStringAsync($"https://api.web.360kan.com/v1/filter/list?catid={(int)cat}&size={size}&cat={tag}");
                var result = JsonSerializer.Deserialize<ResultDto>(jsonResult, _jsonSerializerOptions);
                if (!result.IsSuccess)
                {
                    return new ResultDto<List<ExcitingRecommendationsDto>>
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }
                var obj = JsonNode.Parse(jsonResult);
                var jsonData = obj["data"]["movies"].ToJsonString();

                var data = JsonSerializer.Deserialize<List<ExcitingRecommendationsDto>>(jsonData, _jsonSerializerOptions);
                return new ResultDto<List<ExcitingRecommendationsDto>>
                {
                    Code = 200,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<List<ExcitingRecommendationsDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public async Task<ResultDto<List<RecommendDto>>> GetTops(TopType type)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var jsonResult = await client.GetStringAsync($"https://api.web.360kan.com/v1/rank?cat={(int)type}");
                var result = JsonSerializer.Deserialize<ResultDto<List<RecommendDto>>>(jsonResult, _jsonSerializerOptions);
                if (!result.IsSuccess)
                {
                    return new ResultDto<List<RecommendDto>>
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new ResultDto<List<RecommendDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                };
                return result;
            }
        }

        public async Task<ResultDto> GetDetail()
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var jsonResult = await client.GetStringAsync($"");

                var result = JsonSerializer.Deserialize<ResultDto>(jsonResult);
                if (!result.IsSuccess)
                {
                    return new ResultDto
                    {
                        Code = 500,
                        Message = result.Message
                    };
                }

                return new ResultDto
                {
                    Code = 200,
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }
    }
}
