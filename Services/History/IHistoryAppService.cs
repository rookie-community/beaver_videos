using Beaver.Services.History.Dtos;

namespace Beaver.Services.History
{
    /// <summary>用户播放记录相关业务。</summary>
    public interface IHistoryAppService
    {
        /// <summary>获取某用户的播放记录列表（按最近观看倒序）。</summary>
        Task<List<PlayHistoryDto>> GetListAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 记录 / 刷新一次播放：同一用户同一部影视只保留一条，
        /// 已存在则更新集数与访问时间，不存在则新增。
        /// </summary>
        Task RecordAsync(Guid userId, RecordPlayHistoryDto input, CancellationToken cancellationToken = default);
    }
}
