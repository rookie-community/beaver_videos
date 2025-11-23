using Beaver.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Area("System")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IIdentityRoleAppService _roleAppService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IIdentityRoleAppService roleAppService, ILogger<RoleController> logger)
        {
            _roleAppService = roleAppService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(GetIdentityRolesInput model)
        {
            var roleResult = await _roleAppService.GetListAsync(model);
            return Ok(roleResult.ToLayuiResult());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IdentityRoleCreateDto model)
        {
            var role = _roleAppService.CreateAsync(model);
            return Ok(role);
        }

        public IActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPut]
        public IActionResult Edit(Guid id, IdentityRoleUpdateDto model)
        {
            var role = _roleAppService.UpdateAsync(id, model);
            return Ok(role);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roleAppService.DeleteAsync(id);
            return Ok();
        }
    }
}
