using System.ComponentModel;

namespace Beaver.Movies
{
    /// <summary>
    /// 影视类型
    /// </summary>
    public enum MovieType
    {
        /// <summary>
        /// 总榜/首页
        /// </summary>
        [Description("总榜")]
        Default = 1,

        /// <summary>
        /// 电影
        /// </summary>
        [Description("电影")]
        Film = 2,

        /// <summary>
        /// 电视剧
        /// </summary>
        [Description("电视剧")]
        Teleplay = 3,

        /// <summary>
        /// 综艺
        /// </summary>
        [Description("综艺")]
        Variety = 4,

        /// <summary>
        /// 动漫
        /// </summary>
        [Description("动漫")]
        Anime = 5,

        /// <summary>
        /// 儿童
        /// </summary>
        [Description("儿童")]
        Children = 6,

        /// <summary>
        /// 所有类型/经典
        /// </summary>
        [Description("所有类型")]
        General = 7
    }
}
