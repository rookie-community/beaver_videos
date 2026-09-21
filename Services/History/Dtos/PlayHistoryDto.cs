using Beaver.Services.Movies;

namespace Beaver.Services.History.Dtos
{
    /// <summary>播放记录列表项。</summary>
    public class PlayHistoryDto
    {
        public Guid Id { get; set; }

        /// <summary>影视编号。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称（记录时的快照）。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址（记录时的快照）。</summary>
        public string? Cover { get; set; }

        /// <summary>最近一次观看的集数。</summary>
        public int EpisodeIndex { get; set; }

        /// <summary>UTC 访问时间（最近一次观看）。</summary>
        public DateTime CreationTime { get; set; }
    }
}
