using Beaver.Account;
using Beaver.Dtos;
using Beaver.Models;
using Beaver.Wallpapers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;

namespace Beaver.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : AbpController
    {
        private readonly AbpSignInManager _signInManager;
        private readonly IdentityUserManager _userManager;
        private readonly IBingWallpaperService _bingWallpaperService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AbpSignInManager signInManager, IdentityUserManager userManager, IBingWallpaperService bingWallpaperService, IWebHostEnvironment environment, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _bingWallpaperService = bingWallpaperService;
            _environment=environment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            //创建默认用户
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser(
                    Guid.NewGuid(),
                    "admin",
                    "admin@example.com"
                );
                await _userManager.CreateAsync(adminUser, "1q2w3E*");
                // 你可以在这里为用户分配角色
            }

            //ViewData["ReturnUrl"] = returnUrl;
            var bgIndex = Random.Shared.Next(8);
            var imgResult = await _bingWallpaperService.GetWallpaper(bgIndex, cancellationToken);
            if (imgResult.IsSuccess)
            {
                ViewBag.LoginImage = imgResult.ValueOrDefault;
            }
            return View(new LoginDto
            {
                UserName =_environment.IsDevelopment() ? "admin" : string.Empty,
                ReturnUrl = returnUrl,
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDto model, string? returnUrl = null)
        {
            try
            {
                var localReturnUrl = model.ReturnUrl ?? returnUrl ?? "/";
                var result = await _signInManager.PasswordSignInAsync(
                    model.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    ModelState.AddModelError(nameof(model.Password), "登录失败：请检查账号或密码后重试！");
                    return View(model);
                }

                // 防止开放重定向攻击
                if (!Url.IsLocalUrl(localReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(localReturnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(model.UserName), $"程序异常：{ex.Message}");
                return View(model);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.User.Identity?.Name;
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"用户{userName}退出登录");
            return Redirect("/");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(SettingPasswordDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //校验失败
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                // 验证旧密码是否正确
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    // 密码修改成功
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // 处理错误
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                var msg = $"修改密码失败：{ex.Message}";
                return Ok(new ResultDto
                {
                    Code = 0,
                    //Msg = msg
                });
            }
        }
    }
}
