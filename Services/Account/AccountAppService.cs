using Beaver.Data;
using Beaver.Entities;
using Beaver.Services.Account.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Beaver.Services.Account
{
    public class AccountAppService : IAccountAppService, ITransientDependency
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountAppService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> ValidateCredentialsAsync(string? userName, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

            if (user is null || !user.IsEnabled)
            {
                return null;
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password ?? string.Empty);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // 哈希参数升级时顺带把库里的哈希换成新格式
            if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, password ?? string.Empty);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return user;
        }

        public async Task TouchLastLoginAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
            {
                return;
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<(User? User, string ErrorMessage)> RegisterAsync(RegisterDto input, CancellationToken cancellationToken = default)
        {
            var userName = input.UserName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return (null, "请填写用户名。");
            }

            // admin 是内置管理员账号，不允许被注册占用
            if (string.Equals(userName, AppSeedData.DefaultUserName, StringComparison.OrdinalIgnoreCase))
            {
                return (null, "该用户名为系统保留，请更换一个。");
            }

            if (await _dbContext.Users.AnyAsync(x => x.UserName == userName, cancellationToken))
            {
                return (null, "该用户名已被注册，请更换一个。");
            }

            var user = new User(Guid.NewGuid())
            {
                UserName = userName,
                DisplayName = string.IsNullOrWhiteSpace(input.DisplayName) ? userName : input.DisplayName.Trim(),
                IsEnabled = true,
                // 自助注册的账号一律是普通用户，进不了后台
                IsAdmin = false
                // CreationTime 由 AppDbContext.SaveChangesAsync 统一填充
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, input.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return (user, string.Empty);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
            {
                return (false, "登录状态已失效，请重新登录。");
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword ?? string.Empty);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return (false, "原密码不正确。");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword ?? string.Empty);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return (true, string.Empty);
        }
    }
}
