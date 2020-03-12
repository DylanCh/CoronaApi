using Microsoft.AspNetCore.Mvc;

namespace CoronaApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return Content("<div>Corona API</div>", "text/html");
            return Redirect("~/swagger/v1/swagger.json");
        }
    }
}
