using Beaver.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class UserController : AbpController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(GetUserListDto model)
        {
            var result = await _userService.GetListAsync(model);
            return Ok(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto model)
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
        public async Task<IActionResult> Edit(Guid id, UpdateUserDto model)
        {
            var result = await _userService.UpdateAsync(id, model);
            return Ok(result);
        }
    }
}
