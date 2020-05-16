using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModernPizzaApi.Models
{
    public class StolikModel
    {
        [BsonElement("KodRestauracji")]
        public string KodRestauracji { get; set; }
        [BsonElement("NumerStolika")]
        public int NumerStolika { get; set; }
    }
}