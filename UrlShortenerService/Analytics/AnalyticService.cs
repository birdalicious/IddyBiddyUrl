using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace UrlShortenerService.Analytics
{
    public class AnalyticService
    {
        private IMongoCollection<Analytic> _analyticsCollection;

        private readonly List<Analytic> _buffer = new();

        public int BufferSize { get; init; } = 2;

        public AnalyticService(IMongoCollection<Analytic> analytics, string writeConcernLevel = "1")
        {
            _analyticsCollection = analytics.WithWriteConcern(MapWriteConcerLevel(writeConcernLevel));
        }

        public Task LogAccessAsync(string LinkId)
        {
            return AddToBuffer(new Analytic
            {
                LinkId = LinkId,
                LinkOwnerId = null,
                DateTime = DateTimeOffset.UtcNow
            });
        }
        private WriteConcern MapWriteConcerLevel(string level)
        {
            if (level.ToLower() == "majority")
            {
                return WriteConcern.WMajority;
            }

            if (int.TryParse(level, out var count))
            {
                return new WriteConcern(Math.Max(count, 0));
            }

            return WriteConcern.WMajority;
        }

        private async Task AddToBuffer(Analytic analytic)
        {
            _buffer.Add(analytic);
            if(_buffer.Count >= BufferSize) 
            {
                await FlushBuffer();
            }
        }

        private async Task FlushBuffer()
        {
            await _analyticsCollection.InsertManyAsync(_buffer);
            _buffer.Clear();
        }
    }
}
