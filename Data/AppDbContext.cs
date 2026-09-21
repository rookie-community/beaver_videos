using Beaver.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Auditing;

namespace Beaver.Data
{
    /// <summary>
    /// 应用数据上下文。直接继承 <see cref="DbContext"/>（而不是 ABP 的 AbpDbContext），
    /// 实体映射与种子数据全部走 IEntityTypeConfiguration（含 HasData），
    /// 建库由 EnsureCreated 按当前模型一次性完成，不使用 EF Core 迁移。
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 原生 DbContext 没有审计拦截器，创建时间在这里统一补：
            // 只处理 Added 且尚未赋值的 ABP IHasCreationTime 实体（如继承 CreationAuditedEntity 的 User）
            foreach (var entry in ChangeTracker.Entries<IHasCreationTime>())
            {
                if (entry.State == EntityState.Added && entry.Entity.CreationTime == default)
                {
                    // ABP 把 CreationTime 声明为 protected set（由审计拦截器写入），业务代码无法直接赋值，
                    // 这里改走 EF Core 的属性入口，等价于审计拦截器的做法。
                    entry.Property(nameof(IHasCreationTime.CreationTime)).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 实体映射集中在 Data/Configurations 下，按程序集自动装配
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
