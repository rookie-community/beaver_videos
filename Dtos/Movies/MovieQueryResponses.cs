using System.Text.Json.Serialization;

namespace BeaverVideos.Dtos.Movies
{
    /// <summary>
    /// 影视搜索结果
    /// </summary>
    public class MovieQueryResponses
    {
        [JsonPropertyName("cat_id")]
        public string CatId { get; set; } = null!;

        public string Id { get; set; } = null!;

        [JsonPropertyName("en_id")]
        public string EnId { get; set; } = null!;

        [JsonPropertyName("cat_name")]
        public string CatName { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string Cover { get; set; } = null!;

        public Coverinfo CoverInfo { get; set; } = new Coverinfo();
        public string TitleTxt { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Titlealias { get; set; } = null!;
        public string Year { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string[] Area { get; set; } = Array.Empty<string>();
        public string[] Tag { get; set; } = Array.Empty<string>();
        public string Score { get; set; } = null!;
        public int QualityLv { get; set; }
        public int Pos { get; set; }
        public string[] ActList { get; set; } = Array.Empty<string>();
        public string[] DirList { get; set; } = Array.Empty<string>();
        public string ActName { get; set; } = null!;
        public string DirName { get; set; } = null!;
        public string[] VipSite { get; set; } = Array.Empty<string>();
        public int Vip { get; set; }

        [JsonPropertyName("video_status")]
        public string VideoStatus { get; set; } = null!;
        public Playlinks Playlinks { get; set; } = new Playlinks();
        public string Outc { get; set; } = null!;
        public object[] minilist { get; set; } = Array.Empty<string>();
        public string C { get; set; } = null!;
        public string SeriesSite { get; set; } = null!;
        public object[] seriesPlaylinks { get; set; } = Array.Empty<string>();

        [JsonPropertyName("is_serial")]
        public int IsSerial { get; set; }
    }

    public class Coverinfo
    {
        public string Quality { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Score { get; set; } = null!;
        public string Txt { get; set; } = null!;
    }

    public class Playlinks
    {
        public string M1905 { get; set; } = null!;
        public string YouKu { get; set; } = null!;
        public string QiYi { get; set; } = null!;
        public string QQ { get; set; } = null!;
        public string DouYin { get; set; } = null!;
        public string Bilibili1 { get; set; } = null!;
        public string Sohu { get; set; } = null!;
        public string Xigua { get; set; } = null!;
        public string Cntv { get; set; } = null!;
        public string Leshi { get; set; } = null!;
        public string Imgo { get; set; } = null!;
    }

}
