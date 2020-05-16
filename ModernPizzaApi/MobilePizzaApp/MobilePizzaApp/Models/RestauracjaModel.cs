using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    class RestauracjaModel
    {
        [JsonProperty("ObjectID")]
        public String ObjectId { get; set; }
        [JsonProperty("KodRestauracji")]
        public string KodRestauracji { get; set; }
        [JsonProperty("Stolik")]
        public List<StolikModel> Stolik { get; set; }
        [JsonProperty("XGeoLocalization")]
        public double XGeoLocalization { get; set; }
        [JsonProperty("YGeoLocalization")]
        public double YGeoLocalization { get; set; }
    }
}
