using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        
        public static String EncryotPw(String pw)
        {
            var EncyrptedPW = String.Empty;
            using (var MD5Encryptor = MD5.Create())
            {
                var tempBytes = MD5Encryptor.ComputeHash(Encoding.UTF8.GetBytes(pw + "someRandomText"));
                StringBuilder sb = new StringBuilder();
                tempBytes.ToList().ForEach(x => sb.Append(x.ToString("x2")));
                EncyrptedPW = sb.ToString();
            }
            return EncyrptedPW;
        }
    }
}
