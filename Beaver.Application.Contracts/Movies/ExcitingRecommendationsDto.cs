namespace Beaver.Movies
{
    /// <summary>
    /// 精彩推荐
    /// </summary>
    public class ExcitingRecommendationsDto
    {
        public string CdnCover { get; set; } = null!;
        public string CdnVcover { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string Cover { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool Payment { get; set; }
        public string Title { get; set; } = null!;
    }
}
