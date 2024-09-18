using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeaverVideos.Dto
{
    /// <summary>
    /// 猜你喜欢
    /// </summary>
    public class RecommendDto
    {
        public string Title { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// /// <summary>
        /// 更新信息
        /// </summary>
        /// </summary>
        public string UpInfo { get; set; }
        public string Doubanscore { get; set; }
        public int Id { get; set; }
        public int Cat { get; set; }
        public string Pv { get; set; }
        public string Cover { get; set; }
        public string Url { get; set; }
        public string Percent { get; set; }

        /// <summary>
        /// 影视编号
        /// </summary>
        [JsonPropertyName("ent_id")]
        public string EntId { get; set; }

        /// <summary>
        /// 标签、分类
        /// </summary>
        public List<string> Moviecat { get; set; } = new List<string>();

        /// <summary>
        /// 是否需要会员
        /// </summary>
        public bool Vip { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime Pubdate { get; set; }
    }
}
