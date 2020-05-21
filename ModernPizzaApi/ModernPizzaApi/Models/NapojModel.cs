using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModernPizzaApi.Utils;
namespace ModernPizzaApi.Models
{
    public class NapojModel : IPrzedmiotTransakcji
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Nazwa")]
        public String Nazwa { get; set; }
        [BsonElement("Cena")]
        public double Cena { get; set; }
        [BsonElement("WalidacjaWieku")]
        public Boolean WalidujWiek { get; set; }

        [BsonElement("Obraz")]
        public byte[] Obraz { get; set; }
        public NapojModel()
        {
            ObjectId = Utillities.getHexGuid();
            Nazwa = "Przykladowy napoj";
            Cena = 5.0F;

            //Do usuniecia
            Random rnd = new Random();
            //Koniec usuniecia 

            WalidujWiek = rnd.Next(0, 1) == 1;
        }

        public double PobierzCene()
        {
            return Cena;
        }

        public bool WymagaWalidacji()
        {
            return WalidujWiek;
        }
    }
}
