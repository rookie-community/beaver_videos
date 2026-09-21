using Beaver.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beaver.Data.Configurations
{
    public class PlayHistoryConfiguration : IEntityTypeConfiguration<PlayHistory>
    {
        public void Configure(EntityTypeBuilder<PlayHistory> builder)
        {
            builder.ToTable($"{DbTablePrefix.App}PlayHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntId).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Cover).HasMaxLength(512);

            // 同一用户对同一部影视只保留一条，重新观看时更新集数与访问时间（去重依赖该唯一约束）
            builder.HasIndex(x => new { x.UserId, x.EntId, x.CatType }).IsUnique();

            // 播放记录列表按「用户 + 访问时间倒序」查询
            builder.HasIndex(x => new { x.UserId, x.CreationTime });
        }
    }
}
