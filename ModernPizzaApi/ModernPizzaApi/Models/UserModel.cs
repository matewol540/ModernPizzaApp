using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class UserModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Imie")]
        public String Imie { get; set; }
        [BsonElement("Nazwisko")]
        public String Nazwisko { get; set; }
        [BsonElement("Mail")]
        public String Mail { get; set; }
        [BsonElement("Haslo")]
        public String Password { get; set; }
    }
}
