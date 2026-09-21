namespace Beaver.Services.Movies.Dtos
{
    public class MovieDetailViewModel
    {
        public string Title { get; set; } = null!;

        public string EntId { get; set; } = null!;

        /// <summary>
        /// 标签、分类
        /// </summary>
        public List<string> Moviecategory { get; set; } = new List<string>();

        public string Description { get; set; } = null!;

        /// <summary>
        /// 导演
        /// </summary>
        public List<string> Director { get; set; } = new List<string>();

        public CatType CatType { get; set; }

        public bool Vip { get; set; }

        /// <summary>
        /// 当前播放链接
        /// </summary>
        public string CurrentPlayUrl { get; set; } = null!;

        /// <summary>
        /// 当前剧集
        /// </summary>
        public int CurrentIndex { get; set; } = 1;

        /// <summary>
        /// 播放列表
        /// </summary>
        public List<AllepidetailItem> PlayLinksDetail = new List<AllepidetailItem>();

        /// <summary>
        /// 当前选中数据源类型
        /// </summary>
        public PlayLinkSites CurrentPlayLink { get; set; }

        /// <summary>
        /// 数据源类型
        /// </summary>
        public List<PlayLinkSites> PlayLinkSites { get; set; } = new List<PlayLinkSites>();

        /// <summary>
        /// 精彩推荐
        /// </summary>
        public List<ExcitingRecommendationsDto> Recommends = new List<ExcitingRecommendationsDto>();
    }
}
