// Ignore Spelling: Mongo

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Diagnostics;
using UrlShortenerService.Analytics;
using UrlShortenerService.Mappings;
using UrlShortenerService.Routing;

namespace UrlShortenerService
{
    public static class UrlShortenerServicesBuilder
    {
        public static IServiceCollection AddMongoDbClient(this IServiceCollection services)
        {
            return services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var connectionString = s.GetRequiredService<IConfiguration>().GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });
        }
        public static IServiceCollection AddMongoDbCollections(this IServiceCollection services)
        {
            services.AddScoped<IMongoCollection<Mapping>>(s => {
                var c = s.GetRequiredService<IMongoClient>().GetDatabase("IddyBiddy").GetCollection<Mapping>("mappings");

                c.Indexes.CreateOne(new CreateIndexModel<Mapping>("{ ShortLink: 1 }", new CreateIndexOptions { Unique = true }));
                return c;
            });

            services.AddSingleton<IMongoCollection<Analytic>>(s =>
                s.GetRequiredService<IMongoClient>().GetDatabase("IddyBiddy").GetCollection<Analytic>("analytics"));

            return services;
        }

        public static IServiceCollection AddAnalyticsService(this IServiceCollection services)
        {
            services.AddSingleton<AnalyticsBuffer>(s =>
            {
                var bufferSize = 1;
                int.TryParse(s.GetRequiredService<IConfiguration>().GetSection("Analytics")["BufferSize"], out bufferSize);
                var writeConcern = s.GetRequiredService<IConfiguration>().GetSection("Analytics")["WriteConcern"] ?? "majority";
                var collection = s.GetRequiredService<IMongoCollection<Analytic>>();
                
                return new AnalyticsBuffer(collection, writeConcern, bufferSize);
            });

            services.AddScoped<AnalyticService>();
            return services;
        }

        public static IServiceCollection AddUrlShorteningServices(this IServiceCollection services)
        {
            services.AddMongoDbClient();
            services.AddMongoDbCollections();

            services.AddAnalyticsService();
            services.AddScoped<RoutingService>();
            services.AddScoped<MappingService>();

            return services;
        }
    }
}
