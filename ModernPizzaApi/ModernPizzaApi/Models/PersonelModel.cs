using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ModernPizzaApi.Models
{
    public class PersonelModel
    {
        [BsonId]
        public ObjectId ObjectID { get; set; }
        [BsonElement("Imie")]
        public String Imie { get; set; }
        [BsonElement("Nazwisko")]
        public String Nazwisko { get; set; }
        [BsonElement("Stanowisko")]
        public String Stanowisko { get; set; }
        [BsonElement("Stawka")]
        public double Stawka { get; set; }
        
    }
}
