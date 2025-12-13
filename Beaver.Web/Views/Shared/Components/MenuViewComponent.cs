using Beaver.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beaver.Views.Shared.Components
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var currentPath = ViewContext.HttpContext.Request.Path;

            var menus = new List<MenuItemDto>
            {
                new MenuItemDto
                {
                    Name = "控制台",
                    Path = "/Workbench/Dashboard",
                    Icon = "layui-icon-app",
                },
                new MenuItemDto
                {
                    Name = "系统管理",
                    Icon = "layui-icon-set",
                    Children = new List<MenuItemDto>
                    {
                        new MenuItemDto
                        {
                            Name = "组织机构",
                            Path = "/System/OrganizationUnit",
                            Icon = "layui-icon-link",
                        },
                        new MenuItemDto
                        {
                            Name = "角色",
                            Path = "/System/Role",
                            Icon = "layui-icon-component",
                        },
                        new MenuItemDto
                        {
                            Name = "用户",
                            Path = "/System/User",
                            Icon = "layui-icon-link",
                        },
                        new MenuItemDto
                        {
                            Name = "声明类型",
                            Path = "/System/ClaimType",
                            Icon = "layui-icon-link",
                        },
                        new MenuItemDto
                        {
                            Name = "安全日志",
                            Path = "/System/SecurityLog",
                            Icon = "layui-icon-link",
                        },
                    }
                },
            };

            ViewBag.CurrentPath = currentPath;
            return View(menus);
        }
    }
}
