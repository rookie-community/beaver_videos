using Beaver.Services.Favorites.Dtos;
using Beaver.Services.Movies;

namespace Beaver.Services.Favorites
{
    /// <summary>用户收藏相关业务。</summary>
    public interface IFavoriteAppService
    {
        /// <summary>判断某用户是否已收藏该影视（详情页收藏按钮的初始状态）。</summary>
        Task<bool> IsFavoritedAsync(Guid userId, string entId, CatType catType, CancellationToken cancellationToken = default);

        /// <summary>
        /// 收藏 / 取消收藏：已收藏则删除，未收藏则新增。
        /// 返回操作后的收藏状态与提示信息。
        /// </summary>
        Task<(bool IsFavorited, string Message)> ToggleAsync(Guid userId, ToggleFavoriteDto input, CancellationToken cancellationToken = default);

        /// <summary>获取某用户的收藏列表（按收藏时间倒序）。</summary>
        Task<List<FavoriteDto>> GetListAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
