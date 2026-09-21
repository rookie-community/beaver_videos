using Beaver.Models;
using FluentResults;
using Volo.Abp.Application.Dtos;

namespace Beaver.Extensions
{
    public static class LayuiDtoExtensions
    {
        public static LayuiResultDto<List<T>> ToLayuiResult<T>(this PagedResultDto<T> result) where T : class
        {
            return new LayuiResultDto<List<T>>()
            {
                Code = 0,
                Message = "success",
                Data = result.Items.ToList(),
                Count = result.TotalCount
            };
        }

        public static LayuiResultDto ToLayuiResult(this Result result)
        {
            return new LayuiResultDto
            {
                Code = result.IsSuccess ? 0 : 500,
                Message = result.IsSuccess ? string.Join("; ", result.Successes.Select(s => s.Message)) : string.Join("; ", result.Errors.Select(e => e.Message))
            };
        }

        public static LayuiResultDto<T> ToLayuiResult<T>(this Result<T> result, long count = 0) where T : class
        {
            return new LayuiResultDto<T>
            {
                Code = result.IsSuccess ? 0 : 500,
                Message = result.IsSuccess ? string.Join("; ", result.Successes.Select(s => s.Message)) : string.Join("; ", result.Errors.Select(e => e.Message)),
                Data = result.ValueOrDefault,
                Count = count
            };
        }
    }
}
