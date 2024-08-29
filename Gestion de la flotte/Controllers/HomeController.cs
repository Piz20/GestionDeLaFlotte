using Gestion_de_la_flotte.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gestion_de_la_flotte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

/// <summary>
/// Commentaire
/// </summary>
/// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
