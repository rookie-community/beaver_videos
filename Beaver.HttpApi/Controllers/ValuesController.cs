using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Beaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : AbpControllerBase
    {
        [HttpGet()]
        public IActionResult GetHello()
        {
            return Ok("Hello from Beaver.HttpApi!");
        }
    }
}
