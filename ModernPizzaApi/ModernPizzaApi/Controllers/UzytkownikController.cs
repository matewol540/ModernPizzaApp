using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using ModernPizzaApi.Models;

namespace ModernPizzaApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownikController : ControllerBase
    {


        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<String> AuthUser([FromBody] UserModel UserModel)
        {
            var TempUser = DBConnector.AuthLoggingUser(UserModel.Mail, UserModel.Haslo);
            if (TempUser == null)
                return new BadRequestResult();
            return TempUser.Token;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] UserModel UserModel)
        {
            var result = await DBConnector.DodajUzytkownika(UserModel);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpGet("auth")]
        public async Task<ActionResult<UserModel>> GetUserCredits()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(' ').Last();
            var result = await DBConnector.PobierzUzytkownika(token);
            result.Haslo = String.Empty;
            return Ok(result);
        }
        [HttpPut]
        public async Task<Boolean> EditUser([FromBody]UserModel User)
        {
            var result = await DBConnector.AktualizujUzytkownika(User);
            return result;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete()]
        public void DeleteUser([FromBody] UserModel user)
        {
            DBConnector.UsunUzytkownik(user);
        }
    }
}
