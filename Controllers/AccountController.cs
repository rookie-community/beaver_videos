using Beaver.Data;
using Beaver.Entities;
using Beaver.Models;
using Beaver.Security;
using Beaver.Services.Account;
using Beaver.Services.Account.Dtos;
using Beaver.Services.Wallpapers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : AbpController
    {
        private readonly IAccountAppService _accountAppService;
        private readonly IBingWallpaperService _bingWallpaperService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountAppService accountAppService,
            IBingWallpaperService bingWallpaperService,
            IWebHostEnvironment environment,
            ILogger<AccountController> logger)
        {
            _accountAppService = accountAppService;
            _bingWallpaperService = bingWallpaperService;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            await SetLoginBackgroundAsync(cancellationToken);

            var model = new LoginDto
            {
                ReturnUrl = returnUrl,
            };

            if (_environment.IsDevelopment())
            {
                model.UserName = AppSeedData.DefaultUserName;
                model.Password = AppSeedData.DefaultPassword;
            }

            return View(model);
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
                var localReturnUrl = model.ReturnUrl ?? returnUrl;

                var user = await _accountAppService.ValidateCredentialsAsync(model.UserName, model.Password);
                if (user is null)
                {
                    _logger.LogInformation("用户 {UserName} 登录失败。", model.UserName);
                    ModelState.AddModelError(nameof(model.Password), "登录失败：请检查账号或密码后重试！");
                    await SetLoginBackgroundAsync(default);
                    return View("Login", model);
                }

                // Cookie 认证：写入登录票据（替代原来的 AbpSignInManager）
                await SignInAsync(user, model.RememberMe);
                await _accountAppService.TouchLastLoginAsync(user.Id);

                // 防止开放重定向攻击
                if (!string.IsNullOrWhiteSpace(localReturnUrl) && !Url.IsLocalUrl(localReturnUrl))
                {
                    localReturnUrl = null;
                }

                return Redirect(ResolveRedirectUrl(user.IsAdmin, localReturnUrl));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(model.UserName), $"程序异常：{ex.Message}");
                await SetLoginBackgroundAsync(default);
                return View("Login", model);
            }
        }

        /// <summary>
        /// 注册页：自助注册的账号一律是普通用户，没有后台权限。
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            await SetLoginBackgroundAsync(cancellationToken);
            ViewBag.ReturnUrl = returnUrl;
            return View(new RegisterDto());
        }

        /// <summary>
        /// 注册：成功后直接签发登录票据并进入前台
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var (user, errorMessage) = await _accountAppService.RegisterAsync(model, cancellationToken);
                    if (user is not null)
                    {
                        await SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("新用户 {UserName} 注册成功。", user.UserName);

                        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(nameof(model.UserName), errorMessage);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(model.UserName), $"程序异常：{ex.Message}");
                }
            }

            await SetLoginBackgroundAsync(cancellationToken);
            ViewBag.ReturnUrl = returnUrl;
            return View("Register", model);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.User.Identity?.Name;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("用户 {UserName} 退出登录", userName);
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
                if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    return Ok(new LayuiResultDto { Code = 500, Message = "登录状态已失效，请重新登录。" });
                }

                var result = await _accountAppService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                return Ok(new LayuiResultDto
                {
                    Code = result.Succeeded ? 0 : 500,
                    Message = result.Succeeded ? "密码修改成功" : result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return Ok(new LayuiResultDto
                {
                    Code = 500,
                    Message = $"修改密码失败：{ex.Message}"
                });
            }
        }

        /// <summary>登录页 / 注册页共用的随机 Bing 壁纸</summary>
        private async Task SetLoginBackgroundAsync(CancellationToken cancellationToken)
        {
            var bgIndex = Random.Shared.Next(8);
            var imgResult = await _bingWallpaperService.GetWallpaper(bgIndex, cancellationToken);
            if (imgResult.IsSuccess)
            {
                ViewBag.LoginImage = imgResult.ValueOrDefault;
            }
        }

        /// <summary>签发 Cookie 票据，管理员额外带上 Admin 角色声明</summary>
        private async Task SignInAsync(User user, bool isPersistent)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.IsAdmin ? AppRoles.Admin : AppRoles.User),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = isPersistent });
        }

        /// <summary>
        /// 登录成功后的落点：管理员默认进后台控制台，普通用户回 ReturnUrl（没有则回前台首页）。
        /// </summary>
        private static string ResolveRedirectUrl(bool isAdmin, string? returnUrl)
        {
            // 指向后台区域的地址只有管理员才值得回跳，否则登录后会立刻被拒
            static bool IsBackstage(string url)
                => url.StartsWith("/system", StringComparison.OrdinalIgnoreCase)
                   || url.StartsWith("/workbench", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(returnUrl) && (isAdmin || !IsBackstage(returnUrl!)))
            {
                return returnUrl!;
            }

            return isAdmin ? "/Workbench/Dashboard" : "/Home/Index";
        }
    }
}
