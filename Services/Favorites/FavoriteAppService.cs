using Beaver.Data;
using Beaver.Entities;
using Beaver.Services.Favorites.Dtos;
using Beaver.Services.Movies;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Beaver.Services.Favorites
{
    public class FavoriteAppService : IFavoriteAppService, ITransientDependency
    {
        private const int TitleMaxLength = 256;

        private readonly AppDbContext _dbContext;

        public FavoriteAppService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsFavoritedAsync(Guid userId, string entId, CatType catType, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entId))
            {
                return false;
            }

            return await _dbContext.Favorites
                .AnyAsync(x => x.UserId == userId && x.EntId == entId && x.CatType == catType, cancellationToken);
        }

        public async Task<(bool IsFavorited, string Message)> ToggleAsync(Guid userId, ToggleFavoriteDto input, CancellationToken cancellationToken = default)
        {
            var entId = input.EntId?.Trim();
            if (string.IsNullOrWhiteSpace(entId))
            {
                return (false, "缺少影视编号，无法收藏。");
            }

            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.EntId == entId && x.CatType == input.CatType, cancellationToken);

            // 已收藏 -> 取消收藏
            if (favorite is not null)
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return (false, "已取消收藏。");
            }

            var title = input.Title?.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                title = entId;
            }
            else if (title.Length > TitleMaxLength)
            {
                title = title[..TitleMaxLength];
            }

            _dbContext.Favorites.Add(new Favorite(Guid.NewGuid())
            {
                UserId = userId,
                EntId = entId,
                CatType = input.CatType,
                Title = title,
                Cover = input.Cover
                // CreationTime 由 AppDbContext.SaveChangesAsync 统一填充
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return (true, "收藏成功。");
        }

        public async Task<List<FavoriteDto>> GetListAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Favorites
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new FavoriteDto
                {
                    Id = x.Id,
                    EntId = x.EntId,
                    CatType = x.CatType,
                    Title = x.Title,
                    Cover = x.Cover,
                    CreationTime = x.CreationTime
                })
                .ToListAsync(cancellationToken);
        }
    }
}
