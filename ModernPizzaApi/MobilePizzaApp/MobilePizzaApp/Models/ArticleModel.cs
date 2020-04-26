using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    public class ArticleModel
    {
        [JsonProperty("objectid")]
        public String ObjectId { get; set; }
        [JsonProperty("Tytul")]
        public String Tytul { get; set; }
        [JsonProperty("Zawartosc")]
        public String Zawartosc { get; set; }
        [JsonProperty("Data")]
        public DateTime Data { get; set; }
    }
}
