using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class PizzaModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String PizzaID { get; set; }
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("Lista_Skladnikow")]
        public List<String> Lista_Skladnikow { get; set; }
        [BsonElement("Cena")]
        public double Cena_Pizzy { get; set; }
        public PizzaModel()
        {
            Nazwa = "Random pizza";
            Lista_Skladnikow = new List<string>();
            Lista_Skladnikow.Add("Pomidor");
            Lista_Skladnikow.Add("Szynka");
            Lista_Skladnikow.Add("Ser feta");
        }
        private String getHexGuid()
        {
            byte[] ba = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "").Remove(24);
        }
    }
}
