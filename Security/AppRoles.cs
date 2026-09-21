namespace Beaver.Security
{
    /// <summary>
    /// 角色名常量。
    /// 登录成功时写入票据的 Role Claim，后台区域再用 [Authorize(Roles = ...)] 收口。
    /// </summary>
    public static class AppRoles
    {
        /// <summary>管理员：可访问 /System、/Workbench 后台区域。</summary>
        public const string Admin = "Admin";

        /// <summary>普通用户：注册后的默认角色，只能使用前台功能。</summary>
        public const string User = "User";
    }
}
