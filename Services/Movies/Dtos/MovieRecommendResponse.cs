using System.Text.Json.Serialization;

namespace Beaver.Services.Movies.Dtos
{
    /// <summary>
    /// 猜你喜欢
    /// </summary>
    public class MovieRecommendResponse
    {
        public string Title { get; set; } = null!;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Comment { get; set; } = null!;

        /// <summary>
        /// 更新信息
        /// </summary>
        public string UpInfo { get; set; } = null!;

        public string Doubanscore { get; set; } = null!;
        public int Id { get; set; }
        public int Cat { get; set; }
        public string Pv { get; set; } = null!;
        public string Cover { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Percent { get; set; } = null!;

        /// <summary>
        /// 影视编号
        /// </summary>
        [JsonPropertyName("ent_id")]
        public string EntId { get; set; } = null!;

        /// <summary>
        /// 标签、分类
        /// </summary>
        public List<string> Moviecat { get; set; } = new List<string>();

        public bool Vip { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// 上架时间
        /// </summary>
        public string Pubdate { get; set; } = null!;
    }
}
