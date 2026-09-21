using Beaver.Services.Movies;

namespace Beaver.Services.Favorites.Dtos
{
    /// <summary>收藏 / 取消收藏的入参（详情页收藏按钮提交）。</summary>
    public class ToggleFavoriteDto
    {
        /// <summary>影视编号。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称，作为收藏快照保存。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址，作为收藏快照保存。</summary>
        public string? Cover { get; set; }
    }
}
