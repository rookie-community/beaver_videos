using Beaver.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beaver.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable($"{DbTablePrefix.App}Favorites");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntId).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Cover).HasMaxLength(512);

            // 同一用户对同一部影视只允许一条收藏记录，收藏 / 取消收藏依赖该唯一约束去重
            builder.HasIndex(x => new { x.UserId, x.EntId, x.CatType }).IsUnique();

            // 收藏列表按「用户 + 收藏时间倒序」查询
            builder.HasIndex(x => new { x.UserId, x.CreationTime });
        }
    }
}
