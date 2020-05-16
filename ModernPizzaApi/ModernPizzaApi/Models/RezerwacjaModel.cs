using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModernPizzaApi.Models
{
    public class RezerwacjaModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }

        [BsonElement("User")]
        public String User { get; set; }

        [BsonElement("StartRezerwacji")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartRezerwacji { get; set; }

        [BsonElement("KoniecRezerwacji")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime KoniecRezerwacji { get; set; }

        [BsonElement("Stolik")]
        public StolikModel Stolik { get; set; }
    }
}
