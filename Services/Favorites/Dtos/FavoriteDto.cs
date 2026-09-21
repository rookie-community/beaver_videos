using Beaver.Services.Movies;

namespace Beaver.Services.Favorites.Dtos
{
    /// <summary>收藏列表项。</summary>
    public class FavoriteDto
    {
        public Guid Id { get; set; }

        /// <summary>影视编号。</summary>
        public string EntId { get; set; } = string.Empty;

        /// <summary>影视类型。</summary>
        public CatType CatType { get; set; }

        /// <summary>影视名称（收藏时的快照）。</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>封面地址（收藏时的快照）。</summary>
        public string? Cover { get; set; }

        /// <summary>UTC 收藏时间。</summary>
        public DateTime CreationTime { get; set; }
    }
}
