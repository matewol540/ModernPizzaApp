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
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("StartRezerwacji")]
        public String StartRejestracji { get; set; }
        [BsonElement("KoniecRejestracji")]
        public double KoniecRejestracji { get; set; }
        [BsonElement("Stolik")]
        public StolikModel Stolik;

    }
}
