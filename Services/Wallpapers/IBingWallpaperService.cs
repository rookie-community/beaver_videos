using Beaver.Services.Wallpapers.Dtos;
using FluentResults;

namespace Beaver.Services.Wallpapers
{
    public interface IBingWallpaperService
    {
        Task<Result<List<BingImageDto>>> GetWallpaper(BingWallpaperRequestDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取壁纸
        /// </summary>
        /// <param name="day">0：今天，1：昨天，值范围：0~7</param>
        /// <returns></returns>
        Task<Result<BingImageDto>> GetWallpaper(int day, CancellationToken cancellationToken = default);
    }
}
