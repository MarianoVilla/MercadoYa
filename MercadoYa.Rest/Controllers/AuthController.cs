using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
using MercadoYa.Rest.Logic;
using MercadoYa.Rest.Mock;
using Microsoft.AspNetCore.Authorization;
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
        readonly IAuthenticator Auth;
        readonly IMyPasswordHasher Hasher;
        string InvalidUserMessage = "Invalid email and/or password.";

        public AuthController(IValidator Validator, IDatabase Database, IAuthenticator Auth, IMyPasswordHasher Hasher)
        {
            this.Validator = Validator;
            this.Database = Database;
            this.Auth = Auth;
            this.Hasher = Hasher;
        }
        [HttpPost]
        [Route("users/addstore")]
        public IActionResult AddStoreUser([FromBody] FullStoreUser User)
        {
            if (ValidEmailPassword(User.Email, User.Password))
            {
                var Credentials = (UserCredentials)Hasher.SecureCredentials(User.Email, User.Username, User.Password);
                User.Uid = Database.AddStoreUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);
        }
        [HttpPost]
        [Route("users/addcustomer")]
        public IActionResult AddCustomerUser([FromBody] FullCustomerUser User)
        {
            if (ValidEmailPassword(User.Email, User.Password))
            {
                var Credentials = (UserCredentials)Hasher.SecureCredentials(User.Email, User.Username, User.Password);
                User.Uid = Database.AddCustomerUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);

        }
        [HttpPost]
        [Route("users/signin")]
        public IActionResult SignIn([FromBody] UserCredentials Credentials)
        {
            if (!ValidEmailPassword(Credentials.Email, Credentials.Password))
                return UnprocessableEntity();
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