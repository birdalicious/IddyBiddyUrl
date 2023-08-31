using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace UrlShortenerService.Analytics
{
    public class AnalyticsBuffer : IAsyncDisposable
    {
        public int BufferSize { get; init; }

        private ConcurrentBag<Analytic> _buffer = new();

        private IMongoCollection<Analytic> _analyticsCollection;

        public AnalyticsBuffer(IMongoCollection<Analytic> analyticsCollection, string writeConcern = "majority", int bufferSize = 1) 
        {
            _analyticsCollection = analyticsCollection.WithWriteConcern(MapWriteConcernLevel(writeConcern));
            BufferSize = bufferSize;
        }

        public async Task Add(Analytic analytic)
        {
            _buffer.Add(analytic);
            if (_buffer.Count >= BufferSize)
            {
                await Flush();
            }
        }

        public async ValueTask Flush()
        {
            await _analyticsCollection.InsertManyAsync(_buffer);
            _buffer.Clear();
        }

        public ValueTask DisposeAsync()
        {
            return Flush();
        }
        private WriteConcern MapWriteConcernLevel(string level)
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
    }
}
