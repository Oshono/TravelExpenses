using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelExpenses.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            return View("/Views/Home/Index.cshtml");
        }
    }
}
