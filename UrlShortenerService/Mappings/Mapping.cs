﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortenerService.Mappings
{
    public record Mapping
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public required string ShortLink { get; set; }

        public required string Url { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? LinkOwnerId { get; set; }

        public required bool IsActive { get; set; }
    }
}
