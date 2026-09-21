using Beaver.Data;
using Beaver.Entities;
using Beaver.Services.History.Dtos;
using Beaver.Services.Movies;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Beaver.Services.History
{
    public class HistoryAppService : IHistoryAppService, ITransientDependency
    {
        private const int TitleMaxLength = 256;

        private readonly AppDbContext _dbContext;

        public HistoryAppService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PlayHistoryDto>> GetListAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PlayHistories
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new PlayHistoryDto
                {
                    Id = x.Id,
                    EntId = x.EntId,
                    CatType = x.CatType,
                    Title = x.Title,
                    Cover = x.Cover,
                    EpisodeIndex = x.EpisodeIndex,
                    CreationTime = x.CreationTime
                })
                .ToListAsync(cancellationToken);
        }

        public async Task RecordAsync(Guid userId, RecordPlayHistoryDto input, CancellationToken cancellationToken = default)
        {
            var entId = input.EntId?.Trim();
            if (string.IsNullOrWhiteSpace(entId))
            {
                return;
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

            var history = await _dbContext.PlayHistories
                .FirstOrDefaultAsync(x => x.UserId == userId && x.EntId == entId && x.CatType == input.CatType, cancellationToken);

            if (history is null)
            {
                history = new PlayHistory(Guid.NewGuid())
                {
                    UserId = userId,
                    EntId = entId,
                    CatType = input.CatType,
                    Title = title,
                    Cover = input.Cover,
                    EpisodeIndex = input.EpisodeIndex
                    // CreationTime 由 AppDbContext.SaveChangesAsync 统一填充
                };
                _dbContext.PlayHistories.Add(history);
            }
            else
            {
                // 重新观看：刷新快照与最新集数，并把访问时间更新为当前 UTC
                history.Title = title;
                history.Cover = input.Cover;
                history.EpisodeIndex = input.EpisodeIndex;
                _dbContext.Entry(history).Property(nameof(PlayHistory.CreationTime)).CurrentValue = DateTime.UtcNow;
                _dbContext.PlayHistories.Update(history);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
