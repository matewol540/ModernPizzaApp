using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class TransakcjaModel
    {
        [BsonId]
        public ObjectId objectId { get; set; }

        [BsonElement("DataTranskacji")]
        public DateTime DataTranskacji{ get; set; }
        [BsonElement("Kwota")]
        public double Kwota { get; set; }
        [BsonElement("FormaPlatnosci")]
        public String FormaPlatnosci { get; set; }
        [BsonElement("Produkty")]
        public List<String> Produkty { get; set; }
    }
}
