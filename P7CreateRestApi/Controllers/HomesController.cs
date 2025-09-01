using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous] 
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Welcome",
                serverTimeUtc = DateTime.UtcNow
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")] 
        public IActionResult Admin()
        {
            return Ok(new
            {
                message = "Admin access granted.",
                serverTimeUtc = DateTime.UtcNow
            });
        }
    }
}
