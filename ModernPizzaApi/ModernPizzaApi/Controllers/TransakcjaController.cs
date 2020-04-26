using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransakcjaController : ControllerBase
    {
        [HttpGet("Otwarte")]
        public ActionResult<List<TransakcjaModel>> PobierzOtwarteZamowienia()
        {
            var result = DBConnector.PobierzOtwarteZamowienia();
            return result;
        }
        [HttpGet("Walidacja")]
        public ActionResult<List<TransakcjaModel>> PobierzWalidacjaZamowienia()
        {
            var result = DBConnector.PobierzDoWalidacji();
            return result;
        }


        [HttpPost("Dodaj")]
        public String DodajZamowienie([FromBody]TransakcjaModel Transakcja)
        {
            Transakcja = new TransakcjaModel();
            var response = DBConnector.DodajZamowienie(Transakcja);
            return response;
        }
        [HttpPost("Waliduj")]
        public String WalidujWiek([FromBody]TransakcjaModel Transakcja,Boolean PoprawnyWiek)
        {
           String response = DBConnector.WalidujWiek(Transakcja, PoprawnyWiek);
            return response;
        }
        [HttpPost("Anuluj")]
        public String AnulujZamowienie([FromBody]TransakcjaModel Transakcja)
        {
            var response = DBConnector.AnulujZamowienie(Transakcja);
            return response;
        }
    }
}
