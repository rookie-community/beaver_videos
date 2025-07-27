using BeaverVideos.Dtos.Enums;
using BeaverVideos.Dtos.Movies;
using BeaverVideos.Services.Interfaces;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BeaverVideos.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// 影视服务类
        /// </summary>
        public MovieService(IHttpClientFactory httpClientFactory, IDistributedCache cache)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<Result<List<SuggestionResponse>>> QuerySuggestion(string content, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"{nameof(QuerySuggestion)}_{content}".GetHashCode().ToString();
                var cacheBytes = _cache.Get(cacheKey);
                if (cacheBytes != null)
                {
                    var cacheData = JsonSerializer.Deserialize<List<SuggestionResponse>>(cacheBytes);
                    return Result.Ok(cacheData!);
                }

                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.so.360kan.com/suggest.php?kw={content}";
                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent)!;
                    var json = jsonNode?["data"]?["suglist"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<SuggestionResponse>>(json, _jsonSerializerOptions);
                    //缓存数据
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    _cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(data), options);
                    return Result.Ok(data!);
                }
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取“{content}”文本相关数据失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取“{content}”文本相关数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<List<MovieQueryResponses>>> MovieQuery(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"{nameof(MovieQuery)}_{name}".GetHashCode().ToString();
                var cacheBytes = _cache.Get(cacheKey);
                if (cacheBytes != null)
                {
                    var cacheData = JsonSerializer.Deserialize<List<MovieQueryResponses>>(cacheBytes);
                    return Result.Ok(cacheData!);
                }

                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.so.360kan.com/index?kw={name}&pageno=1";
                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent)!;
                    var json = jsonNode?["data"]?["longData"]?["rows"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<MovieQueryResponses>>(json, _jsonSerializerOptions);
                    //缓存数据
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    _cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(data), options);
                    return Result.Ok(data!);
                }

                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取“{name}”影视数据失败：{resultContent}");

            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取“{name}”影视数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<List<ExcitingRecommendationsDto>>> GetRecommend(CatType cat, string? tag = null, int size = 12, CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.web.360kan.com/v1/filter/list?catid={(int)cat}&size={size}";
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    url += $"&cat={tag}";
                }

                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);

                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent);
                    var json = jsonNode?["data"]?["movies"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<ExcitingRecommendationsDto>>(json, _jsonSerializerOptions);
                    return Result.Ok(data!);
                }

                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取{cat}、{tag}影视推荐数据失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取{cat}、{tag}影视推荐数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<List<MovieRecommendResponse>>> GetTops(MovieType movieType, int size = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"{nameof(GetTops)}_{movieType}_{size}".GetHashCode().ToString();
                var cacheBytes = _cache.Get(cacheKey);
                if (cacheBytes != null)
                {
                    var cacheData = JsonSerializer.Deserialize<List<MovieRecommendResponse>>(cacheBytes);
                    return Result.Ok(cacheData!);
                }

                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.web.360kan.com/v1/rank?cat={(int)movieType}";
                if (size > 0)
                {
                    url += $"&size={size}";
                }
                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent)!;
                    var json = jsonNode?["data"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<MovieRecommendResponse>>(json, _jsonSerializerOptions);
                    //缓存数据
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                    _cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(data), options);
                    return Result.Ok(data!);
                }
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取{movieType}排行榜数据失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取{movieType}排行榜数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<MovieDetail>> GetMovieDetail(CatType cat, string endId, PlayLinkSites site, int start = 0, int end = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"{nameof(GetMovieDetail)}_{cat}_{endId}_{site}_{start}_{end}".GetHashCode().ToString();
                var cacheBytes = _cache.Get(cacheKey);
                if (cacheBytes != null)
                {
                    var cacheData = JsonSerializer.Deserialize<MovieDetail>(cacheBytes);
                    return Result.Ok(cacheData!);
                }

                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.web.360kan.com/v1/detail?cat={(int)cat}&id={endId}&site={site}";
                if (start > 0 && end > 0)
                {
                    url += $"&start={start}&end={end}";
                }
                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent)!;
                    var json = jsonNode?["data"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<MovieDetail>(json, _jsonSerializerOptions);
                    //缓存数据
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    _cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(data), options);
                    return Result.Ok(data!);
                }
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取“{endId}”影视详情数据失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取“{endId}”影视详情数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<List<AllepidetailItem>>> GetMovieDetail(CatType cat, string endId, PlayLinkSites site, int total, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = new List<AllepidetailItem>();
                const int limit = 50;
                for (int i = 1; i < total; i+=limit)
                {
                    var start = i;
                    var end = i + limit - 1;
                    if (end > total)
                    {
                        end = total;
                    }

                    var result = await GetMovieDetail(cat, endId, site, start, end, cancellationToken);
                    if (!result.IsSuccess)
                    {
                        //这个接口存在问题，有数据则返回数据
                        if (data.Count != 0)
                        {
                            return Result.Ok(data);
                        }
                        //无数据返回错误信息
                        return Result.Fail(result.Errors);
                    }
                    var temp = result.Value.Allepidetail[$"{site}"];
                    data.AddRange(temp);
                    continue;
                }
                return Result.Ok(data);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取“{endId}”影视详情数据失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<List<MovieCarousel>>> GetCarousel(int blockid = 522, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"{nameof(GetCarousel)}_{blockid}".GetHashCode().ToString();
                var cacheBytes = _cache.Get(cacheKey);
                if (cacheBytes != null)
                {
                    var cacheData = JsonSerializer.Deserialize<List<MovieCarousel>>(cacheBytes);
                    return Result.Ok(cacheData!);
                }

                using var client = _httpClientFactory.CreateClient();
                var url = $"https://api.web.360kan.com/v1/block?blockid={blockid}";
                using var result = await client.GetAsync(url, cancellationToken);
                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var jsonNode = JsonNode.Parse(resultContent)!;
                    var json = jsonNode?["data"]?["lists"]?.ToJsonString() ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<MovieCarousel>>(json, _jsonSerializerOptions);

                    //缓存数据
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                    _cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(data), options);
                    return Result.Ok(data!);
                }
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取轮播数据失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取轮播数据失败：{ex.Message}").CausedBy(ex));
            }
        }
    }
}
