﻿// Ignore Spelling: Mongo

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using UrlShortenerService.Analytics;
using UrlShortenerService.Mappings;

namespace UrlShortenerService
{
    public static class MongoDbServiceBuilder
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
            services.AddScoped<IMongoCollection<Mapping>>(s => 
            {
                var c = s.GetRequiredService<IMongoClient>().GetDatabase("IddyBiddy").GetCollection<Mapping>("mappings");

                c.Indexes.CreateOne(new CreateIndexModel<Mapping>("{ ShortLink: 1 }", new CreateIndexOptions { Unique = true }));
                return c;
            });

            services.AddScoped<IMongoCollection<Analytic>>(s => 
            {
                var c = s.GetRequiredService<IMongoClient>().GetDatabase("IddyBiddy").GetCollection<Analytic>("analytics");

                c.Indexes.CreateMany(new List<CreateIndexModel<Analytic>>
                {
                    new("{ LinkId: 1 }"),
                    new("{ LinkOwnerId: 1 }"),
                });

                return c;
            });

            return services;
        }
    }
}
