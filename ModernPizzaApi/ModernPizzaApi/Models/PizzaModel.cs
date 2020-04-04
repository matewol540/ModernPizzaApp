using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using ModernPizzaApi.Utils;

namespace ModernPizzaApi.Models
{
    public class PizzaModel : IPrzedmiotTransakcji
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("Lista_Skladnikow")]
        public List<String> Lista_Skladnikow { get; set; }
        [BsonElement("Cena")]
        public double Cena { get; set; }
        [BsonElement("Obraz")]
        public String SciezkaDoObrazu;
        [BsonElement("Oznaczenia")]
        public List<String> Oznaczenia; // Ostra, Bez glutenu, Weganska

        public PizzaModel()
        {
            ObjectId = Utillities.getHexGuid();
            Nazwa = "Random pizza";
            Lista_Skladnikow = new List<string>();
            Lista_Skladnikow.Add("Pomidor");
            Lista_Skladnikow.Add("Szynka");
            Lista_Skladnikow.Add("Ser feta");
            Cena = 25.0F;
            SciezkaDoObrazu = @"D:\ModernPizzaRepo\ModernPizzaApi\ModernPizzaApi\Zasoby\fff.png";
        }

        public bool WymagaWalidacji()
        {
            return false;
        }

        public double PobierzCene()
        {
            return Cena;
        }
    }
}
