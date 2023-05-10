using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PayrollApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[ApiVersion("3.0")]

    public class ValuesController : ControllerBase
    {


        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult Get()
        {

            return Ok(new { Msg = "Version 1" });
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetV2()
        {

            return Ok(new { Msg = "Version 2" });
        }

        [HttpGet]
        [Route("getv1")]
        [MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {

            return Ok(new { Msg = "Version 2" });
        }
    }
}
