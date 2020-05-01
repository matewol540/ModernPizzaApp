using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModernPizzaApi.Models
{
    public class StolikModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectID { get; set; }
        [BsonElement("KodRestauracji")]
        public string KodRestauracji { get; set; }
        [BsonElement("NumerStolika")]
        public string NumerStolika { get; set; }
    }
}