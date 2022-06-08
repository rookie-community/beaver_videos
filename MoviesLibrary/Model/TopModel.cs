using MoviesLibrary.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.Model
{
    /// <summary>
    /// 排行榜
    /// </summary>
    public class TopModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public CatType Cat { get; set; }

        /// <summary>
        /// 影视编号
        /// </summary>
        public string EntId => this.PlayUrl!.ToString().Split("/").LastOrDefault()!.Replace(".html", "");

        /// <summary>
        /// 缩略图
        /// </summary>
        public Uri? Cover { get; set; }

        /// <summary>
        /// 播放链接
        /// </summary>
        public Uri? PlayUrl { get; set; }

        /// <summary>
        /// 播放量
        /// </summary>
        public string Pv { get; set; } = "0";
    }
}
