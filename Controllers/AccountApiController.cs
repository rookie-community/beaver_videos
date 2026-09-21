using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountApiController : AbpControllerBase
    {
        [HttpGet()]
        public IActionResult GetHello()
        {
            var result = Result.Ok("Hello from BeaverVideos API!");
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetGuid()
        {
            try
            {
                throw new Exception("异常测试");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message).ToActionResult();
            }
        }
    }
}
