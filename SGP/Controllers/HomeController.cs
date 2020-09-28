using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System.Diagnostics;

namespace SGP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Nome"] = new HomeModel().LerNomeUsuario();
            return View();           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
