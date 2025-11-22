namespace Beaver.Movies
{
    public class IndexViewModel
    {
        /// <summary>
        /// 轮播数据
        /// </summary>
        public List<MovieCarousel> Carousels { get; set; } = new List<MovieCarousel>();

        /// <summary>
        /// 影视
        /// </summary>
        public Dictionary<MovieType, List<MovieRecommendResponse>> Movies { get; set; } = new Dictionary<MovieType, List<MovieRecommendResponse>>();
    }
}
