using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetRefreshTokenDemo.Api.Constants;

namespace NetRefreshTokenDemo.Api.Controllers
{
    
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {

        [HttpGet]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        //[Authorize]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}