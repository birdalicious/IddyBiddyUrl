using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerService.Mappings;

namespace UrlShortenerService.Analytics
{
    public class AnalyticService
    {
        private IMongoCollection<Analytic> _analytics;

        public AnalyticService(IMongoCollection<Analytic> analytics)
        {
            _analytics = analytics.WithWriteConcern(WriteConcern.Unacknowledged);
        }

        public Task LogAccessAsync(string LinkId)
        {
            return _analytics.InsertOneAsync(new Analytic
            {
                LinkId = LinkId,
                LinkOwnerId = null,
                DateTime = DateTimeOffset.UtcNow
            });
        }
    }
}
