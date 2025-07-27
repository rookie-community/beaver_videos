namespace BeaverVideos.Dtos.Movies
{
    /// <summary>
    /// 文本检索提示信息
    /// </summary>
    public class SuggestionResponse
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Word { get; set; } = null!;

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// 编号
        /// </summary>
        public string Info { get; set; } = null!;
    }
}
