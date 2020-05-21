using MobilePizzaApp.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    public class NapojModel : IItemTemplate
    {
        [JsonProperty("ObjectId")]
        public String ObjectId { get; set; }
        [JsonProperty("Nazwa")]
        public String Nazwa { get; set; }
        [JsonProperty("Cena")]
        public double Cena { get; set; }
        [JsonProperty("WalidacjaWieku")]
        public Boolean WalidujWiek { get; set; }

        [JsonProperty("Obraz")]
        public byte[] Obraz { get; set; }
    }
}
