using Volo.Abp.Domain.Entities.Auditing;

namespace Beaver.Entities
{
    /// <summary>
    /// 应用账号。替代原 ABP Identity 的 IdentityUser，只保留登录与改密所需的最小字段。
    /// 继承 ABP 的 <see cref="CreationAuditedEntity{TKey}"/>：主键 Id 与 CreationTime（含 CreatorId）
    /// 都由基类提供，因此不再需要自定义的 IHasCreationTime 契约。
    /// 说明：ABP 的 Id 是 protected set，需要指定主键时请用带参构造函数。
    /// 映射配置见 Data/Configurations/UserConfiguration。
    /// </summary>
    public class User : CreationAuditedEntity<Guid>
    {
        public User()
        {
        }

        public User(Guid id)
        {
            Id = id;
        }

        public string UserName { get; set; } = string.Empty;

        /// <summary>由 IPasswordHasher&lt;User&gt; 生成，不保存明文。</summary>
        public string PasswordHash { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        /// <summary>禁用后无法登录。</summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 是否管理员。只有管理员能进入 /System、/Workbench 后台区域；
        /// 通过注册页自助注册的账号一律为普通用户。
        /// </summary>
        public bool IsAdmin { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Remarks { get; set; }

        /// <summary>UTC，登录成功后更新。</summary>
        public DateTime? LastLoginAt { get; set; }
    }
}
