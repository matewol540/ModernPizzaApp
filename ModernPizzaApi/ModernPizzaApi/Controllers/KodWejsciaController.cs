using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ModernPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KodWejsciaController : ControllerBase
    {
        // GET: api/KodWejscia
        [HttpGet]
        public String Get()
        {
            var result = DBConnector.PobierzKodWejscia(DateTime.Now);
            return result;
        }
        [HttpPost("{id}")]
        public int WalidujKodWejscia(int id,[FromBody]KodWejsciaModel KodWejscia)
        {
            var reponse = DBConnector.WalidujKodWejscia(KodWejscia);
            if (reponse)
                return id;
            return 0;
        }
    }
}
