using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class KlientModel
    {
        [BsonId]
        public ObjectId ObjectID { get; set; }
        [BsonElement("Imie")]
        public String Imie { get; set; }
        [BsonElement("Nazwisko")]
        public double Nazwisko { get; set; }
        [BsonElement("OstatniZakup")]
        public DateTime OstatniZakup { get; set; }
    }
}
