using Beaver.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Area("System")]
    [Authorize]
    public class UserController : AbpController
    {
        private readonly IIdentityUserAppService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IIdentityUserAppService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(GetIdentityUsersInput model)
        {
            var result = await _userService.GetListAsync(model);
            return Ok(result.ToLayuiResult());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityUserCreateDto model)
        {
            var result = await _userService.CreateAsync(model);
            return Ok(result);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userService.GetAsync(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, IdentityUserUpdateDto model)
        {
            var result = await _userService.UpdateAsync(id, model);
            return Ok(result);
        }
    }
}
