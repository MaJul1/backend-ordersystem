using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("Hello World!");
        }
    }
}
