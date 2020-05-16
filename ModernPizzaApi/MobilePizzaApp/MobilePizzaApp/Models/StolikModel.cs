using Newtonsoft.Json;

namespace MobilePizzaApp.Models
{
    [JsonObject("Object")]
    public class StolikModel
    {
        [JsonProperty("KodRestauracji")]
        public string KodRestauracji { get; set; }
        [JsonProperty("NumerStolika")]
        public int NumerStolika { get; set; }
    }
}