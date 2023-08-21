using IddyBiddyUrl.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IddyBiddyUrl
{
    public static class MongoDbServiceBuilder
    {
        public static IServiceCollection AddMongoDbClient(this IServiceCollection services)
        {
            return services.AddScoped<IMongoClient, MongoClient>(s =>
            {
                var connectionString = s.GetRequiredService<IConfiguration>().GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });
        }
        public static IServiceCollection AddMongoDbCollections(this IServiceCollection services)
        {
            services.AddScoped<IMongoCollection<Mapping>>(s => 
                s.GetRequiredService<IMongoClient>().GetDatabase("IddyBiddy").GetCollection<Mapping>("mappings")
            );

            return services;
        }
    }
}
