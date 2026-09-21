using Beaver.Services.Movies;

namespace Beaver.Services.History.Dtos
{
    /// <summary>写入一条播放记录的入参（详情页播放时调用）。</summary>
    public class RecordPlayHistoryDto
    {
        /// <summary>影视编号。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称，作为记录快照保存。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址，作为记录快照保存。</summary>
        public string? Cover { get; set; }

        /// <summary>本次观看的集数，电影为 1。</summary>
        public int EpisodeIndex { get; set; } = 1;
    }
}
