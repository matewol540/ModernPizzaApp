using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;
using MongoDB.Bson;

namespace ModernPizzaApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArtykulController : ControllerBase
    {
        private const int ArtykulCountPerRequest = 4;

        [HttpGet("{LastIndex}")]
        public IEnumerable<ArtykulModel> Get(int LastIndex)
        {
            var ArtykulyList = DBConnector.PobierzArtykulyAsync().Result;
            ArtykulyList = ArtykulyList.OrderBy(x => x.Data).Skip(LastIndex).Take(ArtykulCountPerRequest).ToList();
            return ArtykulyList;
        }

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
