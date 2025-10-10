using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Domain.Abstracts
{
    public abstract class BaseEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; protected set; }
        public bool RecordStatus { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected BaseEntity()
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedAt = DateTime.UtcNow;
            RecordStatus = true;
        }
    }
}
