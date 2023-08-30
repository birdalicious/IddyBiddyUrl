using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace UrlShortenerService.Analytics
{
    public class AnalyticService
    {

        private AnalyticsBuffer _analyticsBuffer;

        public AnalyticService(IMongoCollection<Analytic> analytics, AnalyticsBuffer buffer, string writeConcernLevel = "1")
        {
            _analyticsBuffer = buffer;
        }

        public Task LogAccessAsync(string LinkId)
        {
            return _analyticsBuffer.Add(new Analytic
            {
                LinkId = LinkId,
                LinkOwnerId = null,
                DateTime = DateTimeOffset.UtcNow
            });
        }
    }
}
