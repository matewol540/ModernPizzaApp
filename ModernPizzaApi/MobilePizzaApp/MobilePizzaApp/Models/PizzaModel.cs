using System;
using System.Collections.Generic;
using System.Text;
using MobilePizzaApp.Interface;
using Newtonsoft.Json;
namespace MobilePizzaApp.Models
{
    
    public class PizzaModel : IItemTemplate
    {
        [JsonProperty("objectid")]
        public String ObjectId { get; set; }
        [JsonProperty("nazwa")]
        public String Nazwa { get; set; }
        [JsonProperty("lista_skladnikow")]
        public List<String> Lista_Skladnikow { get; set; }
        [JsonProperty("cena")]
        public double Cena { get; set; }

        [JsonProperty("Obraz")]
        public byte[] Obraz { get; set; }
    }
}
