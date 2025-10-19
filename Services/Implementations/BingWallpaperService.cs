using BeaverVideos.Dtos.BingWallpaper;
using BeaverVideos.Services.Interfaces;
using FluentResults;
using System.Text.Json;
using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;

namespace BeaverVideos.Services.Implementations
{
    public class BingWallpaperService : IBingWallpaperService, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BingWallpaperService(IHttpClientFactory httpClientFactory)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<List<BingImage>>> GetWallpaper(BingWallpaperRequest model, CancellationToken cancellationToken = default)
        {
            try
            {
                //示例： https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN
                using var client = _httpClientFactory.CreateClient();
                var url = $"https://cn.bing.com/HPImageArchive.aspx?format={model.Format}&idx={model.Idx}&n={model.Count}&mkt={model.Mkt}";
                using var result = await client.GetAsync(url, cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
                    var jsonNode = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);
                    var json = jsonNode?["images"] ?? string.Empty;
                    var data = JsonSerializer.Deserialize<List<BingImage>>(json, _jsonSerializerOptions);
                    return Result.Ok(data!);
                }

                var resultContent = await result.Content.ReadAsStringAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = result.ReasonPhrase;
                }
                return Result.Fail($"获取壁纸失败：{resultContent}");
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"获取壁纸失败：{ex.Message}").CausedBy(ex));
            }
        }

        public async Task<Result<BingImage>> GetWallpaper(int day, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetWallpaper(new BingWallpaperRequest
                {
                    Idx = day,
                    Count = 1,
                }, cancellationToken);

                if (result.IsSuccess)
                {
                    var bingBaseUrl = "https://cn.bing.com";
                    var data = result.Value.First();
                    data.Url = $"{bingBaseUrl}{data.Url}";
                    data.Urlbase = $"{bingBaseUrl}{data.Urlbase}";
                    data.Quiz = $"{bingBaseUrl}{data.Quiz}";
                    return Result.Ok(data);
                }
                return Result.Fail(result.Errors);
            }
            catch (Exception ex)
            {
                //在线获取失败，返回本地默认图片
                return Result.Fail(new Error($"获取壁纸失败：{ex.Message}").CausedBy(ex));
            }
        }
    }
}
