using System.Diagnostics;
using BlogAspNet.Web.Models;
using BlogAspNet.Web.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;

        public HomeController(ILogger<HomeController> logger, IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;

        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _blogService.GetHomeViewModel());
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
