using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    public class RezerwacjaModel
    {
        [JsonProperty("ObjectId")]
        public String ObjectId { get; set; }

        [JsonProperty("User")]
        public String User { get; set; }

        [JsonProperty("StartRezerwacji")]
        public DateTime StartRezerwacji { get; set; }

        [JsonProperty("KoniecRezerwacji")]
        public DateTime KoniecRezerwacji { get; set; }

        [JsonProperty("Stolik")]
        public StolikModel Stolik { get; set; }
    }
}
