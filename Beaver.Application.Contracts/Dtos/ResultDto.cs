using System.Text.Json.Serialization;

namespace Beaver.Dtos
{
    public class ResultDto<T> : ResultDto
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    public class ResultDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        [JsonPropertyName("errno")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        public bool IsSuccess => Code == 0 || Code == 200;
    }
}
