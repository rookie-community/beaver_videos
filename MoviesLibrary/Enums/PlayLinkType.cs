using System.ComponentModel;

namespace MoviesLibrary.Enums
{
    public enum PlayLinkType
    {
        /// <summary>
        /// 腾讯视频
        /// </summary>
        [Description("腾讯视频")]
        qq,
        /// <summary>
        /// 爱奇艺
        /// </summary>
        [Description("爱奇艺")]
        qiyi,
        /// <summary>
        /// 芒果TV
        /// </summary>
        [Description("芒果TV")]
        imgo,
        /// <summary>
        /// 乐视
        /// </summary>
        [Description("乐视")]
        leshi,
        /// <summary>
        /// 搜狐
        /// </summary>
        [Description("搜狐")]
        sohu,
        /// <summary>
        /// 1905电影
        /// </summary>
        [Description("1905电影")]
        m1905,
        /// <summary>
        /// 中央电视台
        /// </summary>
        [Description("中央电视台")]
        cntv,
    }
}
