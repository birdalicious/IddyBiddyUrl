using IddyBiddyUrl.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortenerService.Mappings;
using UrlShortenerService.Validation;

namespace IddyBiddyUrl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MappingService _mappingService;

        public HomeController(ILogger<HomeController> logger, MappingService mappingService)
        {
            _logger = logger;
            _mappingService = mappingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/Home/CreateShortUrl")]
        public async Task<IActionResult> CreateShortLinkAsync(CreateShortUrl model)
        {
            ModelState.Clear();

            var createResult = model.GenerateShortLink
                ? await _mappingService.Create(model.Url)
                : await _mappingService.Create(model.Url, model.ShortLink);

            return createResult.Match(
                success => View("Created", new CreateShortUrl { ShortLink = success.ShortLink, Url = success.Url}),
                error =>
                {
                    if (error is ShortLinkNotAvailableException)
                    {
                        ModelState.AddModelError("ShortLink", error.Message);
                    }
                    if (error is UrlNotValidException)
                    {
                        ModelState.AddModelError("Url", error.Message);
                    }

                    return View("Index", model);
                });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}