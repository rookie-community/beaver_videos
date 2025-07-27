using BeaverVideos.Dtos.BingWallpaper;
using FluentResults;

namespace BeaverVideos.Services.Interfaces
{
    public interface IBingWallpaperService
    {
        Task<Result<List<BingImage>>> GetWallpaper(BingWallpaperRequest model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取壁纸
        /// </summary>
        /// <param name="day">0：今天，1：昨天，值范围：0~7</param>
        /// <returns></returns>
        Task<Result<BingImage>> GetWallpaper(int day, CancellationToken cancellationToken = default);
    }
}
