using System.Globalization;
using Volo.Abp.Application.Dtos;

namespace Beaver.Wallpapers
{
    public class BingWallpaperRequestDto : EntityDto
    {
        /// <summary>
        /// 返回数据格式
        /// </summary>
        /// <remarks>
        /// <para>js：json</para>
        /// <para>xml：xml</para>
        /// </remarks>
        public string Format { get; set; } = "js";

        /// <summary>
        /// 请求图片截止天数
        /// </summary>
        /// <remarks>
        /// <para>0 今天、1 昨天...</para>
        /// <para>值范围：0~7</para>
        /// </remarks>
        public int Idx { get; set; } = 0;

        /// <summary>
        /// 返回请求数量
        /// </summary>
        /// <remarks>
        /// <para>值范围：1~8，目前最多一次获取8张</para>
        /// </remarks>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 地区：zh-CN...
        /// </summary>
        public string Mkt { get; set; } = CultureInfo.CurrentCulture.Name;
    }
}
