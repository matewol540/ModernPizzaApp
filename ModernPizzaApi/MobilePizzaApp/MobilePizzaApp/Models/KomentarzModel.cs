using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    class KomentarzModel
    {
        [JsonProperty("ObjectId")]
        public String KomentarzID { get; set; }
        [JsonProperty("Artykulid")]
        public String ArtukulID { get; set; }

        [JsonProperty("Tresc")]
        public String Tresc{ get; set; }
        [JsonProperty("Autor")]
        public String Autor { get; set; }
        [JsonProperty("Data")]
        public DateTime Data { get; set; }
    }
}
