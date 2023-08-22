using Microsoft.AspNetCore.Mvc;
using UrlShortenerService.Mappings;

namespace IddyBiddyUrl.Controllers
{
    public class RedirectController : Controller
    {
        private readonly MappingService _mappingService;
        public RedirectController(MappingService mappingService)
        {
            _mappingService = mappingService;
        }

        [HttpGet("{shortLink}")]
        public async Task<IActionResult> RedirectShortLink(string shortLink)
        {
            var url = await _mappingService.Get(shortLink);
            if (url is null)
            {
                return NotFound();
            }
            return Redirect(url.Url);
        }
    }
}
