using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            var obj = new
            {
                id = 1,
                title = "mazdak"
            };
            return new JsonResult(obj);
        }
    }
}
