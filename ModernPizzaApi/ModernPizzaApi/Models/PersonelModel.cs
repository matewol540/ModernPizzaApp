using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ModernPizzaApi.Models
{
    public class PersonelModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectID { get; set; }

        [BsonElement("Login")]
        public String Login { get; set; }
        [BsonElement("Haslo")]
        public String Haslo { get; set; }

        [BsonElement("Imie")]
        public String Imie { get; set; }

        [BsonElement("Nazwisko")]
        public String Nazwisko { get; set; }

        [BsonElement("Stanowisko")]
        public String Stanowisko { get; set; }
        [BsonElement("Role")]
        public ClaimsIdentity Role { get; internal set; }


        public String Token { get; set; }

        public PersonelModel()
        {
            Login = "admin";
            Haslo = SzyfrujHaslo(Login,"test");
        }
        internal String SzyfrujHaslo(String Login, String Haslo)
        {
            var hash = String.Empty;
            using (var Szyfr = MD5.Create())
            {
                byte[] data = Szyfr.ComputeHash(Encoding.UTF8.GetBytes(Haslo + Login));
                StringBuilder sb = new StringBuilder();
                data.ToList().ForEach(x => sb.Append(x.ToString("x2")));
                hash = sb.ToString();
                Console.WriteLine(hash);
            }
            return hash;
        }
    }
}
