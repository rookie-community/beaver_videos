using System.Text.Json.Serialization;

namespace BeaverVideos.Dtos.Movies
{
    public class MovieDetail
    {
        /// <summary>
        /// 影视Id
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 影视编号
        /// </summary>
        [JsonPropertyName("ent_id")]
        public string EntId { get; set; } = null!;

        public string Comment { get; set; } = null!;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = null!;

        public string Title { get; set; } = null!;

        /// <summary>
        /// 最新集数
        /// </summary>
        public int UpInfo { get; set; } = 0;

        public List<string> Moviecategory { get; set; } = new List<string>();

        /// <summary>
        /// 导演
        /// </summary>
        public List<string> Director { get; set; } = new List<string>();
        public string Pubdate { get; set; } = null!;

        /// <summary>
        /// 地区
        /// </summary>
        public List<string> Area { get; set; } = new List<string>();

        /// <summary>
        /// 演员
        /// </summary>
        public List<string> Actor { get; set; } = new List<string>();
        public string Cdncover { get; set; } = null!;
        public int Total { get; set; }
        public int Rank { get; set; }
        public Dictionary<string, string> Allupinfo { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Playlinks { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<AllepidetailItem>> Allepidetail { get; set; } = new Dictionary<string, List<AllepidetailItem>>();
        public Dictionary<string, PlayLinksDetailObj> PlayLinksDetail { get; set; } = new Dictionary<string, PlayLinksDetailObj>();

        [JsonPropertyName("playlink_sites")]
        public List<string> PlaylinkSites { get; set; } = new List<string>();
        public int ShaoerRank { get; set; }
        public int IsShaoer { get; set; }

        public bool Vip { get; set; }
    }

    public class AllepidetailItem
    {
        public string Id { get; set; } = null!;

        [JsonPropertyName("playlink_num")]
        public string PlaylinkNum { get; set; } = null!;
        public string Url { get; set; } = null!;

        [JsonPropertyName("v_cover")]
        public string Vcover { get; set; } = null!;

        [JsonPropertyName("is_vip")]
        public string IsVip { get; set; } = null!;

        [JsonPropertyName("api_id")]
        public string ApiId { get; set; } = null!;

        [JsonPropertyName("api_video_id")]
        public string ApiVideoId { get; set; } = null!;
    }

    public class PlayLinksDetailObj
    {
        public string Id { get; set; } = null!;

        public string Site { get; set; } = null!;

        public string Status { get; set; } = null!;

        [JsonPropertyName("default_url")]
        public string DefaultUrl { get; set; } = null!;

        [JsonPropertyName("api_id")]
        public string ApiId { get; set; } = null!;

        [JsonPropertyName("api_video_id")]
        public string ApiVideoId { get; set; } = null!;
    }
}
