using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.Models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public String ID { get; }
        [JsonProperty("Imie")]
        public String Imie { get; set; }
        [JsonProperty("Nazwisko")]
        public String Nazwisko { get; set; }
        [JsonProperty("Mail")]
        public String Mail { get; set; }
        [JsonProperty("Haslo")]
        public String Password { get; set; }
        [JsonProperty("Role")]
        public String Role { get; set; }
    }
}
