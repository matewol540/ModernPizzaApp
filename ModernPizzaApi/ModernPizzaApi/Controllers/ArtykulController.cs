using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ModernPizzaApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class ArtykulController : ControllerBase
    {
        private const int ArtykulCountPerRequest = 5;

        [AllowAnonymous]
        [HttpGet("{LastIndex}")]
        public IEnumerable<ArtykulModel> PobierzArtykuly(int LastIndex)
        {
            var ArtykulyList = DBConnector.PobierzArtykulyAsync().Result;
            ArtykulyList = ArtykulyList.OrderByDescending(x => x.Data).Skip(LastIndex).Take(ArtykulCountPerRequest).ToList();
            ArtykulyList.ForEach(x => x.Komentarze = null);
            return ArtykulyList;
        }

        [AllowAnonymous]
        [HttpGet("Komentarz/{ArtykulId}")]
        public IEnumerable<KomentarzModel> PobierzKomentarzeArtykulu(String ArtykulId)
        {
            var Artykul = DBConnector.PobierzArtykulAsync(ArtykulId).Result;
            if (Artykul.Komentarze == null)
                return new List<KomentarzModel>();
            return Artykul.Komentarze;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post([FromBody]ArtykulModel Artykul)
        {
            try
            {
                DBConnector.DodajArtykul(Artykul);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error occured while adding article to DB.");
            }
        }

        [HttpPost("komentarz")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ArtykulModel>> Dodajkomentarz([FromBody]KomentarzModel komment)
        {
            try
            {
                return await DBConnector.DodajKomentarz(komment);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error occured while adding article to DB.");
                return null;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void PutArticle()
        {
            try
            {
                var Artykul = new ArtykulModel()
                {
                    Data = DateTime.Now,
                    Tytul = "Testowy Artykul z innym zdjeciem",
                    Zawartosc = "Jest to jeden z wielu testów ktorer zostana dodane do bazy danych"
                };
                DBConnector.DodajArtykul(Artykul);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error occured while adding article to DB.");
            }
        }
        //// PUT: api/Artykul/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
