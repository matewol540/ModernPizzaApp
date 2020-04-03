using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModernPizzaApi.Utils;

namespace ModernPizzaApi.Models
{
    public class TransakcjaModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String objectId { get; set; }
        [BsonElement("NumerStolika")]
        public int NumerStolika { get; set; }
        [BsonElement("DataTranskacji")]
        public DateTime DataTranskacji{ get; set; }
        [BsonElement("Kwota")]
        public double Kwota { get; set; }
        [BsonElement("FormaPlatnosci")]
        public String FormaPlatnosci { get; set; }
        [BsonElement("Produkty")]
        public List<IPrzedmiotTransakcji> Produkty { get; set; }

        public TransakcjaModel()
        {
            objectId = Utillities.getHexGuid();
            Produkty = new List<IPrzedmiotTransakcji>();
            Produkty.Add(new PizzaModel());
            Produkty.Add(new PizzaModel());
            Produkty.Add(new NapojModel());
            Produkty.ForEach(x => Kwota += x.PobierzCene());
        }
    }
}
