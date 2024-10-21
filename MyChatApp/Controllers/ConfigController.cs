using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("secrets")]
        public IActionResult GetSecrets()
        {
            var secrets = new
            {
                Endpoint = _configuration["ENDPOINT"],
                ApiKey = _configuration["KEY"]
            };

            return Ok(secrets); // Be careful about exposing sensitive info
        }
    }

}
