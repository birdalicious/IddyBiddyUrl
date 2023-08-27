using System.Runtime.Caching;
using UrlShortenerService.Analytics;
using UrlShortenerService.Mappings;

namespace UrlShortenerService.Routing
{
    public class RoutingService
    {
        private MemoryCache _cache;
        private MappingService _mappingService;
        private AnalyticService _analyticService;
        private readonly TimeSpan DefaultTimeToLife = TimeSpan.FromMinutes(10);

        public RoutingService(MemoryCache cache, MappingService mappingService, AnalyticService analyticService)
        {
            _cache = cache;
            _mappingService = mappingService;
            _analyticService = analyticService;
        }

        public async Task<Mapping?> RouteShortLinkAsync(string shortLink)
        {
            var cachedMapping = _cache.GetCacheItem(shortLink);
            if (cachedMapping is not null)
            {
                var cahcedMappingValue = (Mapping)cachedMapping.Value;
                await _analyticService.LogAccessAsync(cahcedMappingValue.Id);
                return cahcedMappingValue;
            }

            var mapping = await _mappingService.GetAsync(shortLink);
            if (mapping is not null)
            {
                await _analyticService.LogAccessAsync(mapping.Id);
                AddToCache(mapping);
            }

            return mapping;
        }

        private bool AddToCache(Mapping mapping)
        {
            return _cache.Add(new CacheItem(mapping.ShortLink, mapping), new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow + DefaultTimeToLife
            });
        }
    }
}
