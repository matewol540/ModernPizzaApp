using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModernPizzaApi.Models;
using MongoDB.Bson;

namespace ModernPizzaApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class PizzaMainController : ControllerBase
    {
        private readonly ILogger<PizzaMainController> _logger;

        public PizzaMainController(ILogger<PizzaMainController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<PizzaModel>> PobierzWszystkie()
        {
            return DBConnector.PobierzWszystkiePizza();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<PizzaModel> PobierzPizze(String id)
        {
            var newId = id;
            var result = DBConnector.PobierzPizza(newId);
            return result;
        }

        [HttpPost]
        public ActionResult<String> DodajPizza([FromBody]PizzaModel Pizza)
        {
            var result = DBConnector.DodajPizzaAsync(Pizza);
            if (result == null)
                return result.Result;
            return result.Result;
        }

        [HttpPut]
        public ActionResult<String> AktualizujPizze([FromBody] PizzaModel Pizza)
        {
            var result = DBConnector.AktualizujPizzaAsync(Pizza);
            return result.Result;
        }

        [HttpDelete]
        public ActionResult<String> UsunPizza([FromBody] PizzaModel Pizza)
        {
            var result = DBConnector.UsunPizzaAsync(Pizza);
            return result.Result;
        }

    }
}
