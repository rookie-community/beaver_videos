using Beaver.Models;
using Beaver.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Areas.System.Controllers
{
    /// <summary>
    /// 角色管理。
    /// 重构前这里调用的是 ABP Identity 的 IIdentityRoleAppService；移除 ABP 的 EF Core / Identity 组件后，
    /// 本页暂保留为页面骨架（Index 与表格数据接口），角色增删改等业务逻辑待接入自建 Role 实体后补齐。
    /// </summary>
    [Area("System")]
    [Authorize(Roles = AppRoles.Admin)]
    public class RoleController : AbpController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search()
        {
            // TODO: 接入自建 Role 实体后返回真实分页数据（Layui 表格约定的 code/msg/count/data）
            return Ok(new LayuiResultDto<List<object>>
            {
                Code = 0,
                Message = "success",
                Data = new List<object>(),
                Count = 0
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            return View();
        }
    }
}
