using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class MenuModel
    {
        [BsonId]
        public ObjectId ObjectID { get; set; }
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("Cena")]
        public double Cena { get; set; }
        [BsonElement("Opis")]
        public String Opis { get; set; }
        [BsonElement("Sklad")]
        public List<String> Sklad { get; set; }
    }
}
