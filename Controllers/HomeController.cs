using WeddingPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
            private BaseContext _context;
 
    public HomeController(BaseContext context)
    {
        _context = context;
    }

        // GET: /Home/
        // [HttpGet]
        // [Route("")]
        // public IActionResult Index()
        // {
        //     return View();
        // }
    }
}
