using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortenerService.Analytics
{
    public record Analytic
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public required DateTimeOffset DateTime { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? LinkOwnerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required string LinkId { get; set; }
    }
}
