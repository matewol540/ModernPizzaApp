using Microsoft.IdentityModel.Tokens;
using ModernPizzaApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ModernPizzaApi
{
    public static class DBConnector
    {
        private static Semaphore _zasoby = new Semaphore(0, 1);
        private static MongoClient dbClient = new MongoClient(Constants.MONGODB_CONNECTION_STR);

        #region PizzaCommands
        public static List<PizzaModel> PobierzWszystkiePizza()
        {
            var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
            var MongoDBKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

            var PustyFiltr = Builders<PizzaModel>.Filter.Empty;
            return MongoDBKolekcja.Find(PustyFiltr).ToList();
        }

        public static PizzaModel PobierzPizza(String id)
        {
            var MongoDBKlient = dbClient.GetDatabase("ModernPizzaDB");
            var PizzaKolekcja = MongoDBKlient.GetCollection<PizzaModel>("Pizza");

            return PizzaKolekcja.Find<PizzaModel>(x => x.PizzaID == id).First();
        }

        public static async Task<string> DodajPizzaAsync(PizzaModel pizza)
        {
            try
            {
                var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
                var PizzaKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");
                await PizzaKolekcja.InsertOneAsync(pizza);
            }
            catch (Exception err)
            {
                return $"Błąd podczas dodawania do bazy danych - {err.Message}";
            }
            return "Dodano";
        }

        public static async Task<String> AktualizujPizzaAsync(PizzaModel pizza)
        {
            var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
            var PizzaKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

            var update = Builders<PizzaModel>.Update.Set("Nazwa", pizza.Nazwa);
            await PizzaKolekcja.UpdateOneAsync(x => x.PizzaID.Equals(pizza.PizzaID), update);

            return "Aktualizowano";
        }

        public static async Task<String> UsunPizzaAsync(PizzaModel pizza)
        {
            var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
            var PizzaKolekcja = MongoDBClient.GetCollection<BsonDocument>("Pizza");


            var filter = Builders<BsonDocument>.Filter.Eq("_id", pizza.PizzaID);

            var res = await PizzaKolekcja.DeleteOneAsync(filter);

            if (res == null)
                return "Błąd podczas usuwania";
            return "usunieto";
        }
        #endregion

        #region PersonelCommands
        internal static List<PersonelModel> PobierzWszystkichPracownikow()
        {
            var MongoDBKlient = dbClient.GetDatabase("ModernPizzaDB");
            var PersonelKolekcja = MongoDBKlient.GetCollection<PersonelModel>("Personel");

            var Filter = Builders<PersonelModel>.Filter.Empty;

            var Personel = PersonelKolekcja.Find<PersonelModel>(Filter).ToList();

            return Personel;
        }
        internal static PersonelModel AutoryzujPersonel(string login, string v)
        {
            var MongoDBKlient = dbClient.GetDatabase("ModernPizzaDB");
            var PersonelKolekcja = MongoDBKlient.GetCollection<PersonelModel>("Personel");

            var Personel = PersonelKolekcja.Find(x => x.Login == login && x.Haslo == v).First();

            if (Personel == null)
                return null;

            var TokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("PizzaDemoKeyAndSomeRandomData");
            var SignKey = new SymmetricSecurityKey(key);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, Personel.ObjectID)
                }),
                Expires = DateTime.UtcNow.AddDays(90),
                SigningCredentials = new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256),
            };

            var Token = TokenHandler.CreateToken(TokenDescriptor);
            Personel.Token = TokenHandler.WriteToken(Token);

            return Personel;
        }
        internal static async Task<String> DodajPracownikaAsync(PersonelModel personel)
        {
            try
            {
                var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
                var PersonelKolekcja = MongoDBClient.GetCollection<PersonelModel>("Personel");
                await PersonelKolekcja.InsertOneAsync(personel);
            }
            catch (Exception err)
            {
                return err.Message;
            }
            return "Dodano";
        }
        #endregion

        #region Menu

        #endregion
    }
}
