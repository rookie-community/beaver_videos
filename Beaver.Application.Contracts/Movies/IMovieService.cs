using Beaver.Dtos.Enums;
using Beaver.Dtos.Movies;
using FluentResults;

namespace Beaver.Movies
{
    public interface IMovieService
    {
        /// <summary>
        /// 搜索提示
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <returns></returns>
        Task<Result<List<SuggestionResponse>>> QuerySuggestion(string content, CancellationToken cancellationToken = default);

        /// <summary>
        /// 影视搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Result<List<MovieQueryResponses>>> MovieQuery(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// 精彩推荐
        /// </summary>
        /// <param name="cat">类型</param>
        /// <param name="tag">标签，例如：冒险</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        Task<Result<List<ExcitingRecommendationsDto>>> GetRecommend(CatType cat, string? tag = null, int size = 18, CancellationToken cancellationToken = default);

        /// <summary>
        /// 排行榜/推荐
        /// </summary>
        /// <param name="movieType">类型</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        Task<Result<List<MovieRecommendResponse>>> GetTops(MovieType movieType, int size = 0, CancellationToken cancellationToken = default);

        Task<Result<MovieDetail>> GetMovieDetail(CatType cat, string endId, PlayLinkSites site, int start = 0, int end = 0, CancellationToken cancellationToken = default);

        Task<Result<List<AllepidetailItem>>> GetMovieDetail(CatType cat, string endId, PlayLinkSites site, int total, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取轮播数据
        /// </summary>
        /// <param name="blockid">数据块Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<List<MovieCarousel>>> GetCarousel(int blockid = default, CancellationToken cancellationToken = default);
    }
}
