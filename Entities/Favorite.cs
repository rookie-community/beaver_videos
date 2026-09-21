using Beaver.Services.Movies;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Beaver.Entities
{
    /// <summary>
    /// 用户收藏的影视。影视数据来自第三方接口（没有本地影视表），
    /// 因此用 EntId + CatType 唯一标识一部影视；Title / Cover 是收藏时刻的快照，
    /// 让收藏列表不必再依赖上游接口即可展示。
    /// 映射配置见 Data/Configurations/FavoriteConfiguration。
    /// </summary>
    public class Favorite : Entity<Guid>, IHasCreationTime
    {
        public Favorite()
        {
        }

        public Favorite(Guid id)
        {
            Id = id;
        }

        /// <summary>收藏人，对应 AppUsers.Id。</summary>
        public Guid UserId { get; set; }

        /// <summary>影视编号（上游接口的 ent_id）。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型（电影 / 电视剧 / 综艺 / 动漫）。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称（收藏时的快照）。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址（收藏时的快照）。</summary>
        public string? Cover { get; set; }

        /// <summary>UTC 收藏时间，由 AppDbContext.SaveChangesAsync 统一填充。</summary>
        public DateTime CreationTime { get; set; }
    }
}
