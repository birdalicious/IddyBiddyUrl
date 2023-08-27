using Microsoft.AspNetCore.Mvc;
using UrlShortenerService.Mappings;
using UrlShortenerService.Routing;

namespace IddyBiddyUrl.Controllers
{
    public class RedirectController : Controller
    {
        private readonly RoutingService _routingService;
        public RedirectController(RoutingService routingService)
        {
            _routingService = routingService;
        }

        [HttpGet("{shortLink}")]
        public async Task<IActionResult> RedirectShortLink(string shortLink)
        {
            var mapping = await _routingService.RouteShortLinkAsync(shortLink);
            if (mapping is null)
            {
                return NotFound();
            }
            return Redirect(mapping.Url);
        }
    }
}
