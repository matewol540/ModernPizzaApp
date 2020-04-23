using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MobilePizzaApp.Models
{
    public class InformationModel
    {
        [JsonProperty("title")]
        public String Tytul_Informacji { get; set; }
        [JsonProperty("title")]
        public String Content { get; set; }
        [JsonProperty("title")]
        public Image Image { get; set; }

    }
}
