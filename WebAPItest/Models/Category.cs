using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPItest.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Count { get; set; }
    }
}
