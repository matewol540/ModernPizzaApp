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
using ModernPizzaApi.Utils;
using ModernPizzaApi.Controllers;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.CodeAnalysis.CSharp;

namespace ModernPizzaApi
{
    public static class DBConnector
    {
        private static MongoClient dbClient = new MongoClient(new MongoClientSettings()
        {
            ConnectionMode = ConnectionMode.Automatic,
            Credential = MongoCredential.CreateCredential("bvjlr3yieol9j03", "u2bcoixvwja8lneywmyo", "LWSdD6FD3x9g4WrT9Dv8"),
            Server = new MongoServerAddress("bvjlr3yieol9j03-mongodb.services.clever-cloud.com", 27017)

        });

        private const String DBName = "bvjlr3yieol9j03";


        public static List<TransakcjaModel> OtwarteZamowienia = new List<TransakcjaModel>();


        public static List<TransakcjaModel> DoWalidacjiZamowienia = new List<TransakcjaModel>();

        #region Artykul
        public static async Task<List<ArtykulModel>> PobierzArtykulyAsync()
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var ArtykulKolekcja = MongoDBClient.GetCollection<ArtykulModel>("Artykuly");
            var PustyFiltr = Builders<ArtykulModel>.Filter.Empty;
            var TempList = await ArtykulKolekcja.FindAsync<ArtykulModel>(PustyFiltr);
            return TempList.ToList();
        }
        internal static async Task<ArtykulModel> PobierzArtykulAsync(String artykulId)
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var ArtykulKolekcja = MongoDBClient.GetCollection<ArtykulModel>("Artykuly");
            var Article = await ArtykulKolekcja.FindAsync<ArtykulModel>(x => x.Id == artykulId);

            return Article.First();
        }

        internal static async Task<ArtykulModel> DodajKomentarz(KomentarzModel komment)
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var ArtykulKolekcja = MongoDBClient.GetCollection<ArtykulModel>("Artykuly");
            var TempArtykul = ArtykulKolekcja.Find(x => x.Id == komment.Artykulid).First();
            if (TempArtykul.Komentarze == null)
                TempArtykul.Komentarze = new List<KomentarzModel>();
            TempArtykul.Komentarze.Add(komment);
            await ArtykulKolekcja.FindOneAndReplaceAsync(x => x.Id == komment.Artykulid, TempArtykul);
            return TempArtykul;
        }
        public static void DodajArtykul(ArtykulModel Artykul)
        {
            Artykul.Obraz = File.ReadAllBytes(@"D:\ModernPizzaRepo\ModernPizzaApi\MobilePizzaApp\MobilePizzaApp\Zasoby\TempPizzeria.jpg");
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var ArtykulKolekcja = MongoDBClient.GetCollection<ArtykulModel>("Artykuly");
            ArtykulKolekcja.InsertOne(Artykul);
        }


        #endregion

        #region PizzaCommands
        public static List<PizzaModel> PobierzWszystkiePizza()
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var MongoDBKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

            var PustyFiltr = Builders<PizzaModel>.Filter.Empty;
            return MongoDBKolekcja.Find(PustyFiltr).ToList();
        }
        //public static PizzaModel PobierzPizza(String id)
        //{
        //    var MongoDBKlient = dbClient.GetDatabase(DBName);
        //    var PizzaKolekcja = MongoDBKlient.GetCollection<PizzaModel>("Pizza");

        //    return PizzaKolekcja.Find<PizzaModel>(x => x.ObjectId == id).First();
        //}

        public static async Task<string> DodajPizzaAsync(PizzaModel pizza)
        {
            try
            {
                var MongoDBClient = dbClient.GetDatabase(DBName);
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
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var PizzaKolekcja = MongoDBClient.GetCollection<PizzaModel>("Pizza");

            var update = Builders<PizzaModel>.Update.Set("Nazwa", pizza.Nazwa);
            await PizzaKolekcja.UpdateOneAsync(x => x.ObjectId.Equals(pizza.ObjectId), update);

            return "Aktualizowano";
        }
        public static async Task<String> UsunPizzaAsync(PizzaModel pizza)
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var PizzaKolekcja = MongoDBClient.GetCollection<BsonDocument>("Pizza");


            var filter = Builders<BsonDocument>.Filter.Eq("_id", pizza.ObjectId);

            var res = await PizzaKolekcja.DeleteOneAsync(filter);

            if (res == null)
                return "Błąd podczas usuwania";
            return "usunieto";
        }
        #endregion

        #region PersonelCommands
        internal static List<PersonelModel> PobierzWszystkichPracownikow()
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var PersonelKolekcja = MongoDBKlient.GetCollection<PersonelModel>("Personel");

            var Filter = Builders<PersonelModel>.Filter.Empty;

            var Personel = PersonelKolekcja.Find<PersonelModel>(Filter).ToList();

            return Personel;
        }
        internal static PersonelModel AutoryzujPersonel(string login, string v)
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
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
            Personel.Haslo = String.Empty;
            return Personel;
        }
        internal static async Task<String> DodajPracownikaAsync(PersonelModel personel)
        {
            try
            {
                var MongoDBClient = dbClient.GetDatabase(DBName);
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

        #region Transakcje
        internal static List<TransakcjaModel> PobierzOtwarteZamowienia()
        {
            return OtwarteZamowienia;
        }
        internal static List<TransakcjaModel> PobierzDoWalidacji()
        {
            return DoWalidacjiZamowienia;
        }
        public static String DodajZamowienie(TransakcjaModel Transakcja)
        {
            try
            {
                if (Transakcja.ProduktyWydane.Where(x => x.WymagaWalidacji()).Count() > 0)
                    DoWalidacjiZamowienia.Add(Transakcja);
                else
                    OtwarteZamowienia.Add(Transakcja);
            }
            catch (Exception err)
            {
                return err.Message;
            }
            return "Dodano zamowienie";
        }
        public static string WalidujWiek(TransakcjaModel transakcja, bool poprawnyWiek)
        {
            try
            {
                if (poprawnyWiek)
                {
                    DoWalidacjiZamowienia.Remove(transakcja);
                    OtwarteZamowienia.Add(transakcja);
                }
                else
                {
                    DoWalidacjiZamowienia.Remove(transakcja);
                }
            }
            catch (Exception err)
            {
                return $"Blad operacji - {err.Message}";
            }
            return "Success";
        }
        public static String AnulujZamowienie(TransakcjaModel transakcja)
        {
            try
            {
                OtwarteZamowienia.Remove(transakcja);
            }
            catch (Exception err)
            {
                return err.Message;
            }
            return "Usunieto zamowienie";
        }
        public static async Task<String> ZakonczZamowienieAsync(TransakcjaModel transakcja)
        {
            try
            {
                var MongoDBClient = dbClient.GetDatabase(DBName);
                var TransakcjaKolekcja = MongoDBClient.GetCollection<TransakcjaModel>("Transakcje");

                await TransakcjaKolekcja.InsertOneAsync(transakcja);
            }
            catch (Exception err)
            {
                return err.Message;
            }
            return $"Zamowienie {transakcja.objectId} zakonczono";
        }

        #endregion

        #region NapojCommands

        #endregion

        #region Uzytkownik 
        public static UserModel AuthLoggingUser(string login, string v)
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var UsersCollection = MongoDBKlient.GetCollection<UserModel>("Uzytkownicy");

            var User = UsersCollection.Find(x => x.Mail == login && x.Haslo == v).First();
            if (User == null)
                return null;
            var TokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("PizzaDemoKeyAndSomeRandomData");
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, User.Mail),
                    new Claim(ClaimTypes.Role, User.Role)
                }),
                Expires = DateTime.Now.AddDays(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var Token = TokenHandler.CreateToken(TokenDescriptor);
            User.Token = TokenHandler.WriteToken(Token);
            User.Haslo = String.Empty;
            return User;
        }
        public async static Task<Boolean> DodajUzytkownika(UserModel userModel)
        {
            var MongoDBClient = dbClient.GetDatabase(DBName);
            var UzytkownicyKolekcja = MongoDBClient.GetCollection<UserModel>("Uzytkownicy");
            var ExistingMail = UzytkownicyKolekcja.Find(x => x.Mail == userModel.Mail);
            if (ExistingMail.CountDocuments() != 0)
                return false;
            else
                await UzytkownicyKolekcja.InsertOneAsync(userModel);
            return true;
        }
        internal async static Task<UserModel> PobierzUzytkownika(String token)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var Claims = TokenHandler.ReadToken(token) as JwtSecurityToken;
            var Mail = Claims.Claims.First(x => x.Type == "unique_name").Value;

            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var UsersCollection = MongoDBKlient.GetCollection<UserModel>("Uzytkownicy");
            var users = await UsersCollection.FindAsync<UserModel>(x => x.Mail == Mail);
            if (users.ToList().Count != 0)
                return users.First();
            return null;
        }
        public async static Task<Boolean> AktualizujUzytkownika(UserModel user)
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var UsersCollection = MongoDBKlient.GetCollection<UserModel>("Uzytkownicy");
                var UpdateUser = (await UsersCollection.FindAsync<UserModel>(x => x.Mail == user.Mail)).First();
                user.ObjectId = UpdateUser.ObjectId;
                await UsersCollection.ReplaceOneAsync<UserModel>(x => x.Mail == user.Mail, user);
                return true;
            }
            catch
            {

            }
            return false;
        }
        public async static void UsunUzytkownik(UserModel user)
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var UsersCollection = MongoDBKlient.GetCollection<UserModel>("Uzytkownicy");
                await UsersCollection.FindOneAndDeleteAsync<UserModel>(x => x.Mail == user.Mail);
            }
            catch (Exception err)
            {

            }
        }
        #endregion

        #region Rezerwacja
        internal async static Task<List<RezerwacjaModel>> PobierzRezerwacjeUzytkownika(String userID)
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
            var Results = await RezerwacjeCollection.FindAsync<RezerwacjaModel>(x => x.User == userID);

            return Results.ToList();
        }
        internal async static Task<Boolean> SprawdzCzyTerminDostepny(RezerwacjaModel rezerwacja)
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
            var result = (await RezerwacjeCollection.FindAsync<RezerwacjaModel>(x => true)).ToList();
            var reserved = result.Where(x => x.Stolik.KodRestauracji == rezerwacja.Stolik.KodRestauracji &&
                x.Stolik.KodRestauracji == rezerwacja.Stolik.KodRestauracji &&
                x.StartRezerwacji <= rezerwacja.StartRezerwacji &&
                x.KoniecRezerwacji >= rezerwacja.StartRezerwacji &&
                x.Status != "Expired");
            return reserved.Any();
        }
        internal async static Task<bool> DodajRezerwacje(RezerwacjaModel rezerwacja)
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
                await RezerwacjeCollection.InsertOneAsync(rezerwacja);
                return true;
            }
            catch (Exception err)
            {

            }
            return false;
        }
        internal async static Task<bool> EdytujRezerwacje(RezerwacjaModel rezerwacja)
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
                await RezerwacjeCollection.FindOneAndReplaceAsync(x => x.ObjectId == rezerwacja.ObjectId, rezerwacja);
                return true;
            }
            catch (Exception err)
            {

            }
            return false;
        }
        internal async static Task<bool> UsunRezerwacje(String rezerwacjaID)
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
                await RezerwacjeCollection.FindOneAndDeleteAsync(x => x.ObjectId == rezerwacjaID);
                return true;
            }
            catch (Exception err)
            {

            }
            return false;
        }
        #endregion

        #region Napoje
        public async static Task<List<NapojModel>> PobierzNapoje()
        {
            var MongoDB = dbClient.GetDatabase(DBName);
            var NapojeKolekcja = MongoDB.GetCollection<NapojModel>("Napoj");
            var TempList = (await NapojeKolekcja.FindAsync<NapojModel>(x => true)).ToList();
            return TempList;
        }
        public async static Task<Boolean> EdytujNapoj(NapojModel tempNapoj)
        {
            var MongoDB = dbClient.GetDatabase(DBName);
            var NapojeKolekcja = MongoDB.GetCollection<NapojModel>("Napoj");
            var TempList = (await NapojeKolekcja.FindOneAndReplaceAsync<NapojModel>(x => tempNapoj.ObjectId == x.ObjectId, tempNapoj));
            if (TempList != null)
                return true;
            return false;

        }

        public async static Task<Boolean> DodajNapoj(NapojModel tempNapoj)
        {
            try
            {
                var MongoDB = dbClient.GetDatabase(DBName);
                var NapojeKolekcja = MongoDB.GetCollection<NapojModel>("Napoj");
                await NapojeKolekcja.InsertOneAsync(tempNapoj);
                return true;
            }
            catch (Exception err)
            {

            }
            return false;
        }

        public async static Task<Boolean> UsunNapoj(NapojModel tempNapoj)
        {
            try
            {
                var MongoDB = dbClient.GetDatabase(DBName);
                var NapojeKolekcja = MongoDB.GetCollection<NapojModel>("Napoj");
                await NapojeKolekcja.DeleteOneAsync(x => x.ObjectId == tempNapoj.ObjectId);
                return true;
            }
            catch (Exception err)
            {

            }
            return false;
        }
        #endregion


        #region Resturacja 
        internal async static Task<List<RestauracjaModel>> PobierzRestauracje()
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var RezerwacjeCollection = MongoDBKlient.GetCollection<RestauracjaModel>("Restauracje");
            return (await RezerwacjeCollection.FindAsync<RestauracjaModel>(x => true)).ToList();
        }
        internal async static void DodajRestauracje(String ID)
        {
            var MongoDBKlient = dbClient.GetDatabase(DBName);
            var RestauracjeCollection = MongoDBKlient.GetCollection<RestauracjaModel>("Restauracje");
            var restTemp = new RestauracjaModel()
            {
                KodRestauracji = ID,
                Stolik = new List<StolikModel>() {
                    new StolikModel() {
                        KodRestauracji = ID,
                        NumerStolika = 1
                    },
                    new StolikModel() {
                        KodRestauracji = ID,
                        NumerStolika = 2
                    },
                    new StolikModel() {
                        KodRestauracji = ID,
                        NumerStolika = 3
                    },
                    new StolikModel() {
                        KodRestauracji = ID,
                        NumerStolika = 4
                    }
                },
                XGeoLocalization = 49.883352,
                YGeoLocalization = 19.493483
            };
            await RestauracjeCollection.InsertOneAsync(restTemp);
        }
        #endregion

        #region CronHandler
        public static void CheckReservagtionsForExpired()
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
                var result = RezerwacjeCollection.Find(x => x.Status == "Planned" && x.StartRezerwacji <= DateTime.Now.AddMinutes(-5.0F)).ToList();

                result.ForEach(x =>
                {
                    x.Status = "Expired";
                    RezerwacjeCollection.FindOneAndReplaceAsync(x1 => x1.ObjectId == x.ObjectId, x);
                });

                result = RezerwacjeCollection.Find(x => x.Status == "Active" && DateTime.Now <= x.KoniecRezerwacji).ToList();

                result.ForEach(x =>
                {
                    x.Status = "Done";
                    RezerwacjeCollection.FindOneAndReplaceAsync(x1 => x1.ObjectId == x.ObjectId, x);
                });
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
            }
        }

        public static void CleanUpReservations()
        {
            try
            {
                var MongoDBKlient = dbClient.GetDatabase(DBName);
                var RezerwacjeCollection = MongoDBKlient.GetCollection<RezerwacjaModel>("Rezerwacje");
                RezerwacjeCollection.DeleteManyAsync(x => x.StartRezerwacji <= DateTime.Now.AddDays(-30F));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
            }
        }


        #endregion

    }
}
