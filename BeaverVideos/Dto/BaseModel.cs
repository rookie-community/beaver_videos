using BeaverVideos.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaverVideos.Dto
{
    /// <summary>
    /// 影视类基类
    /// </summary>
    public class BaseModel
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
        public string EntId { get; set; } = string.Empty;
    }
}
