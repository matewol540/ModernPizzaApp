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
        public DateTime DataTranskacji { get; set; }
        [BsonElement("Kwota")]
        public double Kwota { get; set; }
        [BsonElement("FormaPlatnosci")]
        public String FormaPlatnosci { get; set; }
        [BsonElement("ProduktyWydane")]
        public List<IPrzedmiotTransakcji> ProduktyWydane { get; set; }
        [BsonElement("ProduktyZamowione")]
        public List<IPrzedmiotTransakcji> ProduktyZamowione { get; set; }


        public Boolean WiekAutoryzowany { get; set; }

        public TransakcjaModel()
        {
            objectId = Utillities.getHexGuid();
            ProduktyWydane = new List<IPrzedmiotTransakcji>();
            ProduktyWydane.Add(new PizzaModel());
            ProduktyWydane.Add(new PizzaModel());
            ProduktyWydane.Add(new NapojModel());
            ProduktyWydane.ForEach(x => Kwota += x.PobierzCene());
        }
    }
}
