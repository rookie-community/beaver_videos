using Beaver.Areas.Identity.Models;
using Beaver.Dtos;
using Beaver.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Beaver.Wallpapers;

namespace Beaver.Areas.Identity.Controllers
{
    [Area("Identity")]
    [AutoValidateAntiforgeryToken]
    public class AccountController : AbpController
    {
        private readonly AbpSignInManager _signInManager;
        private readonly IdentityUserManager _userManager;
        private readonly IBingWallpaperService _bingWallpaperService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AbpSignInManager signInManager, IdentityUserManager userManager, IBingWallpaperService bingWallpaperService, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _bingWallpaperService = bingWallpaperService;
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

            ViewData["ReturnUrl"] = returnUrl;
            var bgIndex = Random.Shared.Next(8);
            var imgResult = await _bingWallpaperService.GetWallpaper(bgIndex, cancellationToken);
            if (imgResult.IsSuccess)
            {
                ViewBag.LoginImage = imgResult.ValueOrDefault;
            }
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model, string? returnUrl = null)
        {
            try
            {
                var localReturnUrl = model.ReturnUrl ?? returnUrl;
                ViewData["ReturnUrl"] = localReturnUrl;
                //model.ReturnUrl = HttpContext.Request.Query.Where(x => x.Key == "ReturnUrl").Select(x => x.Value).FirstOrDefault();
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

                //    var user = _freeSql.Queryable<User>().Where(x => x.Account == model.UserName).First();
                //    if (user == null)
                //    {
                //        ModelState.AddModelError(nameof(model.UserName), $"用户“{model.UserName}”不存在！");
                //        return View(model);
                //    }
                //    //实例化一个md5对像
                //    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                //    byte[] bytes = MD5.HashData(Encoding.UTF8.GetBytes(model.Password));
                //    var pwd = Convert.ToBase64String(bytes);

                //    if (pwd != user.Password)
                //    {
                //        ModelState.AddModelError(nameof(model.Password), "密码错误");
                //        return View(model);
                //    }

                //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //    identity.AddClaims(new[]
                //    {
                //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, model.UserName),
                //    new Claim(ClaimTypes.Role, user.Role.ToString()),
                //    new Claim(ClaimTypes.System, "DataAcquisition")
                //});

                //    var principal = new ClaimsPrincipal(identity);

                //    // 登录
                //    var properties = new AuthenticationProperties
                //    {
                //        IsPersistent = model.RememberMe,
                //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                //        AllowRefresh = true
                //    };
                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                //    _logger.LogInformation($"用户{model.UserName}登录成功");
                //    if (Url.IsLocalUrl(model.ReturnUrl))
                //    {
                //        return Redirect(model.ReturnUrl);
                //    }

                //    return Redirect("/");
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
        public async Task<IActionResult> UpdatePassword(SettingPasswordModel model)
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
