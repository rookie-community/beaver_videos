using MoviesLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.Model
{
    /// <summary>
    /// 精彩推荐
    /// </summary>
    public class MovieRecommend: BaseModel
    {
        /// <summary>
        /// 简介
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 缩略图
        /// </summary>
        public Uri? Cover { get; set; }

        /// <summary>
        /// CDN图片地址
        /// </summary>
        public Uri? CdnCover { get; set; }

        /// <summary>
        /// 总集数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 已更新集数
        /// </summary>
        public int UpInfo { get; set; }

        /// <summary>
        /// 是否收费
        /// </summary>
        public bool Vip { get; set; }
    }
}
