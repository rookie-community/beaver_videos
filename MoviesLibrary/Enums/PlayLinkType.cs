using System.ComponentModel;

namespace MoviesLibrary.Enums
{
    public enum PlayLinkType
    {
        /// <summary>
        /// 腾讯视频
        /// </summary>
        [Description("腾讯视频")]
        qq = 1,
        /// <summary>
        /// 爱奇艺
        /// </summary>
        [Description("爱奇艺")]
        qiyi = 2,
        /// <summary>
        /// 优酷
        /// </summary>
        [Description("优酷")]
        youku = 3,
        /// <summary>
        /// 芒果TV
        /// </summary>
        [Description("芒果TV")]
        imgo = 4,
        /// <summary>
        /// 乐视
        /// </summary>
        [Description("乐视")]
        leshi = 5,
        /// <summary>
        /// 搜狐
        /// </summary>
        [Description("搜狐")]
        sohu = 6,
        /// <summary>
        /// 1905电影
        /// </summary>
        [Description("1905电影")]
        m1905 = 7,
        /// <summary>
        /// 中央电视台
        /// </summary>
        [Description("央视网")]
        cntv = 8,
        /// <summary>
        /// 西瓜视频
        /// </summary>
        [Description("西瓜视频")]
        xigua = 9,
        /// <summary>
        /// PPTV聚力
        /// </summary>
        [Description("PPTV聚力")]
        pptv
    }
}
