using BeaverVideos.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaverVideos.Dto
{
    public class MovieDetail : BaseModel
    {
        /// <summary>
        /// 影视Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 最新集数
        /// </summary>
        public int UpInfo { get; set; } = 0;
        /// <summary>
        /// 标签、分类
        /// </summary>
        public List<string> Moviecategory { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 影视简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 导演
        /// </summary>
        public List<string> Director { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public List<string> Area { get; set; }
        /// <summary>
        /// 主演
        /// </summary>
        public List<string> Actor { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public Uri CdnCover { get; set; }
        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double DouBanScore { get; set; }
        /// <summary>
        /// 播放列表
        /// </summary>
        public Dictionary<int, string> PlayLinksDetail { get; set; } = new Dictionary<int, string>();
        /// <summary>
        /// 当前线路
        /// </summary>
        public PlayLinkType ThisPlayLink { get; set; }
        /// <summary>
        /// 线路列表
        /// </summary>
        public List<string> PlayLinkSites { get; set; }
        /// <summary>
        /// 是否需要会员
        /// </summary>
        public bool Vip { get; set; }

        /// <summary>
        /// 精彩推荐
        /// </summary>
        public IEnumerable<MovieRecommend> MovieRecommends { get; set; } = new List<MovieRecommend>();
    }
}
