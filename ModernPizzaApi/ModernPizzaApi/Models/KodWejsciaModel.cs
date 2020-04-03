using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ModernPizzaApi
{
    public class KodWejsciaModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Data")]
        public string Data { get; set; }
        [BsonElement("KodWejscia")]
        public string kodWejscia { get; set; }

        public KodWejsciaModel()
        {

        }
        public KodWejsciaModel(string v, string kodWejscia)
        {
            this.Data = v;
            this.kodWejscia = kodWejscia;
        }


    }
}