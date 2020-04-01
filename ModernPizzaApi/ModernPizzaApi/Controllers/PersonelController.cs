using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : ControllerBase
    {

        private readonly ILogger<PersonelController> _logger;

        public PersonelController(ILogger<PersonelController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<PersonelModel>> PobierzPracowkow()
        {
            var Personel = DBConnector.PobierzWszystkichPracownikow();
            return Personel;
        }

        [AllowAnonymous]
        [HttpPost("Auth")]
        public IActionResult Authenticate([FromBody]PersonelModel Personel)
        {
            var tempUser = DBConnector.AutoryzujPersonel(Personel.Login, Personel.SzyfrujHaslo(Personel.Login, Personel.Haslo));
            if (tempUser == null)
                return new BadRequestResult();
            return new OkResult();
        }

        [HttpPost]
        public ActionResult<String> DodajPracownik([FromBody]PersonelModel personel)
        {
            var result = DBConnector.DodajPracownikaAsync(personel);
            return result.Result;
        }

        [HttpPut]
        public IActionResult AktualizujPracownik([FromBody]PersonelModel personel)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public IActionResult UsunPracownik([FromBody]PersonelModel personel)
        {
            throw new NotImplementedException();
        }

    }
}
