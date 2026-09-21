using Beaver.Models;
using Beaver.Services.Favorites;
using Beaver.Services.Favorites.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Controllers
{
    /// <summary>前台收藏接口：详情页收藏按钮通过 Ajax 调用。</summary>
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class FavoriteController : AbpController
    {
        private readonly IFavoriteAppService _favoriteAppService;

        public FavoriteController(IFavoriteAppService favoriteAppService)
        {
            _favoriteAppService = favoriteAppService;
        }

        /// <summary>收藏 / 取消收藏，返回操作后的最新状态。</summary>
        [HttpPost]
        public async Task<IActionResult> Toggle(ToggleFavoriteDto input, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Ok(new LayuiResultDto { Code = 500, Message = "登录状态已失效，请重新登录。" });
            }

            try
            {
                var (isFavorited, message) = await _favoriteAppService.ToggleAsync(userId, input, cancellationToken);

                return Ok(new LayuiResultDto<FavoriteStateDto>
                {
                    Code = 0,
                    Message = message,
                    Data = new FavoriteStateDto { IsFavorited = isFavorited }
                });
            }
            catch (Exception ex)
            {
                return Ok(new LayuiResultDto
                {
                    Code = 500,
                    Message = $"操作失败：{ex.Message}"
                });
            }
        }
    }
}
