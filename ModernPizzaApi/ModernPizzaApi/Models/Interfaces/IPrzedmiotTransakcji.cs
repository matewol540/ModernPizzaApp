using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi
{
    public interface IPrzedmiotTransakcji
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("Cena")]
        public double Cena { get; set; }
        public Boolean WymagaWalidacji();
        public double PobierzCene();
    }
}
