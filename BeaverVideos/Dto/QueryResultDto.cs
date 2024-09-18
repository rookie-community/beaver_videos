using BeaverVideos.Common.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeaverVideos.Dto
{
    public class QueryResultDto
    {
        public string Id { get; set; }

        [JsonPropertyName("en_id")]
        public string EnId { get; set; }

        [JsonPropertyName("cat_id")]
        public string CatId { get; set; }

        [JsonPropertyName("cat_name")]
        public string CatName { get; set; }
        //public string url { get; set; }
        public string Cover { get; set; }
        public Coverinfo CoverInfo { get; set; }
        //public string titleTxt { get; set; }

        [JsonPropertyName("titleTxt")]
        public string Title { get; set; }
        //public string titlealias { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string[] Area { get; set; }
        public string[] Tag { get; set; }
        public string Score { get; set; }
        //public int qualityLv { get; set; }
        //public int pos { get; set; }
        public string[] ActList { get; set; }
        public string[] DirList { get; set; }
        public string ActName { get; set; }
        public string DirName { get; set; }

        public string[] VipSite { get; set; }
        public int Vip { get; set; }

        [JsonPropertyName("video_status")]
        public string VideoStatus { get; set; }

        /// <summary>
        /// 片源
        /// </summary>
        /// <remarks>Key：<see cref="PlayLinkType"/></remarks>
        public Dictionary<string, string> Playlinks { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 平台
        /// </summary>
        /// <remarks><see cref="PlayLinkType"/></remarks>
        public string SeriesSite { get; set; }

        //public object[] seriesPlaylinks { get; set; }
        //public int is_serial { get; set; }
        //public object[] minilist { get; set; }
        //public string c { get; set; }
        //public string outc { get; set; }
    }

    public class Coverinfo
    {
        public string Txt { get; set; }
    }
}
