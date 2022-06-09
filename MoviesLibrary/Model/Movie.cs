using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.Model
{
    /// <summary>
    /// 影视
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 唯一编码
        /// </summary>
        public string EnId { get; set; } = string.Empty;
        /// <summary>
        /// 类型ID
        /// </summary>
        public int CatId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? CatName { get; set; }
        /// <summary>
        /// 封面URL
        /// </summary>
        public Uri? CoverUrl { get; set; }
        /// <summary>
        /// 封面信息
        /// </summary>
        public Dictionary<string, string>? CoverInfo { get; set; }
        /// <summary>
        /// 影片名称
        /// </summary>
        public string? Title { get; set; }
        public int Year { get; set; }
        /// <summary>
        /// 影片描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public List<string>? Area { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string>? Tag { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 主演
        /// </summary>
        public List<string>? ActList { get; set; }
        /// <summary>
        /// 导演
        /// </summary>
        public List<string>? DirList { get; set; }
        /// <summary>
        /// 是否需要会员
        /// </summary>
        public bool Vip { get; set; }
        /// <summary>
        /// 影视状态
        /// </summary>
        public string? VideoStatus { get; set; }
        /// <summary>
        /// 播放列表
        /// </summary>
        public Dictionary<string, string>? PlayLinks { get; set; }
    }
}
