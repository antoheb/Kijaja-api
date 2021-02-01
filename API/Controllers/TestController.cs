using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "Saoud pue";
        } 
    }
}