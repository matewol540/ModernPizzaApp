using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauracjaController : ControllerBase
    {
        [HttpGet]
        public async Task<List<RestauracjaModel>> Get()
        {
            var Results = await DBConnector.PobierzRestauracje();
            return Results;
        }

        [HttpGet("{id}")]
        public void Get(String id)
        {
            DBConnector.DodajRestauracje(id);
        }

        //// POST: api/Restauracja
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Restauracja/5
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
