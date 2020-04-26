using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.IO;

namespace ModernPizzaApi.Models
{
    public class ArtykulModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Tytul")]
        public String  Tytul { get; set; }
        [BsonElement("Zawartosc")]
        public String Zawartosc { get; set; }
        [BsonElement("Data")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Data { get; set; }
        [BsonElement("Obraz")]
        public byte[] Obraz { get; set; }
    }
}
