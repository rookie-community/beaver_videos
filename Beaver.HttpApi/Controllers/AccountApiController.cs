using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Beaver.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountApiController : AbpControllerBase
    {
        private readonly IIdentityUserAppService _userAppService;

        public AccountApiController(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet()]
        public IActionResult GetHello()
        {
            var result = Result.Ok("Hello from Beaver.HttpApi!");
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
