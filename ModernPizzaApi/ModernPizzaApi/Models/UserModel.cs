using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class UserModel
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("Imie")]
        public String Imie { get; set; }
        [BsonElement("Nazwisko")]
        public String Nazwisko { get; set; }
        [BsonElement("Mail")]
        public String Mail { get; set; }
        [BsonElement("Haslo")]
        public String Haslo { get; set; }
        [BsonElement("Role")]
        public String Role { get; set; }

        public String Token { get; set; }
        internal object SzyfrujHaslo(object login, object haslo)
        {

            var EncyrptedPW = String.Empty;
            using (var MD5Encryptor = MD5.Create())
            {
                var tempBytes = MD5Encryptor.ComputeHash(Encoding.UTF8.GetBytes(haslo + "someRandomText"));
                StringBuilder sb = new StringBuilder();
                tempBytes.ToList().ForEach(x => sb.Append(x.ToString("x2")));
                EncyrptedPW = sb.ToString();
            }
            return EncyrptedPW;
        }
    }
}
