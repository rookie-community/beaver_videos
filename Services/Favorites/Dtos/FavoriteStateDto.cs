namespace Beaver.Services.Favorites.Dtos
{
    /// <summary>收藏状态回执，前端据此刷新收藏按钮。</summary>
    public class FavoriteStateDto
    {
        /// <summary>操作后的收藏状态：true 表示已收藏。</summary>
        public bool IsFavorited { get; set; }
    }
}
