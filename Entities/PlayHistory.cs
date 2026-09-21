using Beaver.Services.Movies;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Beaver.Entities
{
    /// <summary>
    /// 用户播放记录。影视数据来自第三方接口（没有本地影视表），
    /// 因此用 EntId + CatType 唯一标识一部影视；Title / Cover 是记录时刻的快照。
    /// 同一用户对同一部影视只保留最新一条，重新观看时更新集数与访问时间，
    /// 映射配置见 Data/Configurations/PlayHistoryConfiguration。
    /// </summary>
    public class PlayHistory : Entity<Guid>, IHasCreationTime
    {
        public PlayHistory()
        {
        }

        public PlayHistory(Guid id)
        {
            Id = id;
        }

        /// <summary>观看人，对应 AppUsers.Id。</summary>
        public Guid UserId { get; set; }

        /// <summary>影视编号（上游接口的 ent_id）。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型（电影 / 电视剧 / 综艺 / 动漫）。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称（记录时的快照）。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址（记录时的快照）。</summary>
        public string? Cover { get; set; }

        /// <summary>最近一次观看的集数，电影固定为 1。</summary>
        public int EpisodeIndex { get; set; } = 1;

        /// <summary>UTC 访问时间（即最近一次观看时间），由 AppDbContext.SaveChangesAsync 统一填充或刷新。</summary>
        public DateTime CreationTime { get; set; }
    }
}
