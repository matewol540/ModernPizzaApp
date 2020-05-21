using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Interface
{
    public interface IItemTemplate
    {
        [JsonProperty("Nazwa")]
        String Nazwa { get; set; }
        [JsonProperty("Cena")]
        double Cena { get; set; }

        [JsonProperty("Obraz")]
        byte[] Obraz { get; set; }
    }
}
