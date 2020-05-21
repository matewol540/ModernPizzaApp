using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NapojController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<NapojModel>> Get()
        {
            var TempList = await DBConnector.PobierzNapoje();
            return TempList;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NapojModel TempNapoj)
        {
            var result = await DBConnector.DodajNapoj(TempNapoj);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPut()]
        public async Task<ActionResult> Put([FromBody] NapojModel TempNapoj)
        {
            var result = await DBConnector.EdytujNapoj(TempNapoj);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete()]
        public async Task<ActionResult> UsunNapoj([FromBody] NapojModel TempNapoj)
        {
            var result = await DBConnector.UsunNapoj(TempNapoj);
            if (result)
                return Ok();
            return BadRequest();
        }
    }
}
