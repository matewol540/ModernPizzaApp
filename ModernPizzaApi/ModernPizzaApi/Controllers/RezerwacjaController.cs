using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezerwacjaController : ControllerBase
    {
        [HttpGet("{UserID}")]
        public async Task<IEnumerable<RezerwacjaModel>> Get(String UserID)
        {
            var ReservationList = await DBConnector.PobierzRezerwacjeUzytkownika(UserID);
            return ReservationList as IEnumerable<RezerwacjaModel>;
        }
        [HttpPost("Check")]
        public async Task<ActionResult<Boolean>> CheckRezerwacja([FromBody] RezerwacjaModel Rezerwacja)
        {
            if (await DBConnector.SprawdzCzyTerminDostepny(Rezerwacja))
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<RezerwacjaModel>> DodajRezerwacje([FromBody] RezerwacjaModel Rezerwacja)
        {
            if (await DBConnector.DodajRezerwacje(Rezerwacja))
                return Ok(Rezerwacja);
            return BadRequest(null);
        }
        [HttpPut]
        public async Task<ActionResult<RezerwacjaModel>> Put([FromBody] RezerwacjaModel Rezerwacja)
        {
            if (await DBConnector.EdytujRezerwacje(Rezerwacja))
                return Ok(Rezerwacja);
            return BadRequest(null);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(String id)
        {
            if (await DBConnector.UsunRezerwacje(id))
                return Ok();
            return BadRequest();

        }
    }
}
