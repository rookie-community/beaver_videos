using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaverVideos.Common.Enums
{
    /// <summary>
    /// 排行榜类型
    /// </summary>
    public enum TopType
    {
        /// <summary>
        /// 总榜
        /// </summary>
        Default = 1,
        /// <summary>
        /// 电影
        /// </summary>
        Film = 2,
        /// <summary>
        /// 电视剧
        /// </summary>
        Teleplay = 3,
        /// <summary>
        /// 综艺
        /// </summary>
        Variety = 4,
        /// <summary>
        /// 动漫
        /// </summary>
        Anime = 5,
        /// <summary>
        /// 儿童
        /// </summary>
        Children = 6,
        /// <summary>
        /// 所有类型
        /// </summary>
        General = 7
    }
}
