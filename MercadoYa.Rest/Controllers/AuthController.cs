using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
using MercadoYa.Rest.Logic;
using MercadoYa.Rest.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MercadoYa.Rest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IValidator Validator;
        readonly IDatabase Database;
        readonly IAuth Auth;
        readonly IMyPasswordHasher Hasher;
        string InvalidUserMessage = "Invalid email and/or password.";

        public AuthController(IValidator Validator, IDatabase Database, IAuth Auth, IMyPasswordHasher Hasher)
        {
            this.Validator = Validator;
            this.Database = Database;
            this.Auth = Auth;
            this.Hasher = Hasher;
        }
        [HttpPost]
        [Route("users/addstore")]
        public IActionResult AddStoreUser([FromBody] string Email, [FromBody] string Username, [FromBody] string Password, [FromBody] ClientUser User)
        {
            if (ValidEmailPassword(Email, Password))
            {
                var Credentials = (UserCredentials)Hasher.SecureCredentials(Email, Username, Password);
                User.Uid = Database.AddStoreUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);
        }
        [HttpPost]
        [Route("users/addclient")]
        public IActionResult AddClientUser([FromBody] string Email, [FromBody] string Username, [FromBody] string Password, [FromBody] ClientUser User)
        {
            if (ValidEmailPassword(Email, Password))
            {
                var Credentials = (UserCredentials)Hasher.SecureCredentials(Email, Username, Password);
                User.Uid = Database.AddClientUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);

        }
        [HttpPost]
        [Route("users/signin")]
        public IActionResult SignIn([FromBody] UserCredentials Credentials)
        {
            var Uid = Auth.AuthUser(Credentials.Email, Credentials.Password) as string;
            if (Uid is null)
            {
                return Unauthorized();
            }
            Response.Cookies.Append("timestamp", CryptoUtil.EncryptString(Const.CryptoDevKey, DateTime.Now.ToString("yyyyMMddHHmmssFFF")));
            return new JsonResult(new { Uid });
        }
        bool ValidEmailPassword(string Email, string Password)
        {
            return Validator.IsValidEmail(Email) && Validator.IsValidPassword(Password);
        }
    }
}