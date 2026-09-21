using Beaver.Services.Favorites;
using Beaver.Services.Favorites.Dtos;
using Beaver.Services.History;
using Beaver.Services.History.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Controllers
{
    /// <summary>
    /// 前台用户中心：我的收藏 / 播放记录。
    /// 导航菜单 _UserNavPartial 指向这里（area 为空，走默认路由 /User/...）。
    /// </summary>
    [Authorize]
    public class UserController : AbpController
    {
        private readonly IFavoriteAppService _favoriteAppService;
        private readonly IHistoryAppService _historyAppService;

        public UserController(IFavoriteAppService favoriteAppService, IHistoryAppService historyAppService)
        {
            _favoriteAppService = favoriteAppService;
            _historyAppService = historyAppService;
        }

        private static bool TryGetUserId(ClaimsPrincipal principal, out Guid userId)
        {
            return Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }

        /// <summary>我的收藏。</summary>
        public async Task<IActionResult> Favorites(CancellationToken cancellationToken = default)
        {
            if (!TryGetUserId(User, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["title"] = "我的收藏";
            var list = await _favoriteAppService.GetListAsync(userId, cancellationToken);
            return View(list);
        }

        /// <summary>播放记录。</summary>
        public async Task<IActionResult> History(CancellationToken cancellationToken = default)
        {
            if (!TryGetUserId(User, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["title"] = "播放记录";
            var list = await _historyAppService.GetListAsync(userId, cancellationToken);
            return View(list);
        }
    }
}
