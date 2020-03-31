using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ModernPizzaApi
{
    public static class DBConnector
    {
        private static Semaphore _zasoby = new Semaphore(0, 1);
        private static MongoClient dbClient = new MongoClient(Constants.MONGODB_CONNECTION_STR);

        #region PizzaCommands
        public static List<PizzaModel> PobierzWszystkie()
        {
            var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
            var MongoDBKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

            var PustyFiltr = Builders<PizzaModel>.Filter.Empty;
            return MongoDBKolekcja.Find(PustyFiltr).ToList();
        }
        public static PizzaModel PobierzPizza(String id)
        {
            var MongoDBClient = dbClient.GetDatabase("ModernPizzaDB");
            var PizzaKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

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
                return "Błąd podczas dodawania do bazy danych";
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


























        #region KlientCommands

        #endregion

        #region Menu

        #endregion
    }
}
