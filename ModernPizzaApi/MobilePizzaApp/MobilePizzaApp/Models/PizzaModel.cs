using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace MobilePizzaApp.Models
{
    
    public class PizzaModel
    {
        [JsonProperty("objectid")]
        public String ObjectId { get; set; }
        [JsonProperty("nazwa")]
        public String Nazwa { get; set; }
        [JsonProperty("lista_skladnikow")]
        public List<String> Lista_Skladnikow { get; set; }
        [JsonProperty("cena")]
        public double Cena { get; set; }
    }
}
