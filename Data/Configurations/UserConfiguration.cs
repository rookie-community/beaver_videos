using Beaver.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beaver.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{DbTablePrefix.App}Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName).IsRequired().HasMaxLength(64);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(64);
            builder.Property(x => x.PhoneNumber).HasMaxLength(32);
            builder.Property(x => x.Email).HasMaxLength(128);
            builder.Property(x => x.Remarks).HasMaxLength(512);

            builder.HasIndex(x => x.UserName).IsUnique();

            // 种子数据：内置管理员账号。首次初始化（EnsureCreated 按模型建库）时随表结构一并写入，
            // 本项目不做增量迁移，后续结构变更请手动执行 SQL（见 README「建库与结构变更」）。
            //
            // 这里用匿名对象而不是 new User(...)：CreationTime 的 setter 是 protected，
            // 匿名对象按属性名写入种子值，EF 直接把值插入，不经过实体构造函数与属性 setter。
            // 因此种子里的口令哈希必须是确定值，见 AppSeedData.AdministratorPasswordHash。
            builder.HasData(new
            {
                Id = AppSeedData.AdministratorUserId,
                UserName = AppSeedData.DefaultUserName,
                PasswordHash = AppSeedData.AdministratorPasswordHash,
                DisplayName = "超级管理员",
                IsEnabled = true,
                // 内置账号是唯一的管理员来源
                IsAdmin = true,
                CreationTime = AppSeedData.AdministratorCreationTime
            });
        }
    }
}
