using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortenerService.Mappings
{
    public class Mapping
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ShortLink { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
