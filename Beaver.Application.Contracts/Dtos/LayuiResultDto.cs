using System.Text.Json.Serialization;
using Volo.Abp.Application.Dtos;

namespace Beaver.Dtos
{
    public class LayuiResultDto<T> : LayuiResultDto where T : class
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; } = default!;

        [JsonPropertyName("count")]
        public long Count { get; set; }
    }

    public class LayuiResultDto : EntityDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; } = null!;
    }
}
