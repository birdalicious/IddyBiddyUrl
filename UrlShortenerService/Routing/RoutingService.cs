using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Caching;
using UrlShortenerService.Analytics;
using UrlShortenerService.Mappings;

namespace UrlShortenerService.Routing
{
    public class RoutingService
    {
        private IMemoryCache _cache;
        private MappingService _mappingService;
        private AnalyticService _analyticService;
        private readonly TimeSpan DefaultTimeToLife = TimeSpan.FromMinutes(10);

        public RoutingService(IMemoryCache cache, MappingService mappingService, AnalyticService analyticService)
        {
            _cache = cache;
            _mappingService = mappingService;
            _analyticService = analyticService;
        }

        public async Task<Mapping?> RouteShortLinkAsync(string shortLink)
        {
            var cachedMapping = _cache.Get<Mapping>(shortLink);
            if (cachedMapping is not null)
            {
                await _analyticService.LogAccessAsync(cachedMapping.Id);
                return cachedMapping;
            }

            var mapping = await _mappingService.GetAsync(shortLink);
            if (mapping is not null)
            {
                await _analyticService.LogAccessAsync(mapping.Id);
                AddToCache(mapping);
            }

            return mapping;
        }

        private void AddToCache(Mapping mapping)
        {
            _cache.Set(mapping.ShortLink, mapping, DefaultTimeToLife);
        }
    }
}
