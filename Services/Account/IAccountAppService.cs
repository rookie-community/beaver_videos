using Beaver.Entities;
using Beaver.Services.Account.Dtos;

namespace Beaver.Services.Account
{
    /// <summary>
    /// 登录相关的应用服务：替代原来的 ABP Identity（AbpSignInManager / IdentityUserManager）。
    /// </summary>
    public interface IAccountAppService
    {
        /// <summary>校验账号口令，成功返回用户，失败返回 null。</summary>
        Task<User?> ValidateCredentialsAsync(string? userName, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// 自助注册普通用户（无后台权限）。成功返回新用户，失败返回错误信息。
        /// </summary>
        Task<(User? User, string ErrorMessage)> RegisterAsync(RegisterDto input, CancellationToken cancellationToken = default);

        /// <summary>记录最近一次登录时间。</summary>
        Task TouchLastLoginAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>修改口令。</summary>
        Task<(bool Succeeded, string ErrorMessage)> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
    }
}
