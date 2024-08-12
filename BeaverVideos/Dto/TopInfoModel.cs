using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaverVideos.Dto
{
    public class TopInfoModel : TopModel
    {
        /// <summary>
        /// 影视编号
        /// </summary>
        public new string EntId { get; set; } = string.Empty;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 更新信息
        /// </summary>
        public string UpInfo { get; set; } = string.Empty;

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 标签、分类
        /// </summary>
        public List<string> Moviecat { get; set; } = new List<string>();

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// 是否收费
        /// </summary>
        public bool Vip { get; set; }
    }
}
