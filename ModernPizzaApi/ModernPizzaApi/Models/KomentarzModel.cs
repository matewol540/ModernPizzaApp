using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class KomentarzModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Artykulid")]
        public String Artykulid { get; set; }
        [BsonElement("Tresc")]
        public String Tresc { get; set; }
        [BsonElement("Autor")]
        public String Autor { get; set; }
        [BsonElement("Data")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Data { get; set; }
    }
}
