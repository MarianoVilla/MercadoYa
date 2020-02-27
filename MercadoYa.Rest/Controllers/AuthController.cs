using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using MercadoYa.Rest.Logic;
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
        public IActionResult AddStoreUser([FromBody] StoreUser User)
        {
            if (ValidUser(User))
            {
                var Credentials = (UserCredentials)Hasher.GenerateCredentials(User.Email, User.Username, User.Password);
                User.Uid = Database.AddStoreUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);
        }
        [HttpPost]
        [Route("users/addclient")]
        public IActionResult AddClientUser([FromBody] ClientUser User)
        {
            if (ValidUser(User))
            {
                var Credentials = (UserCredentials)Hasher.GenerateCredentials(User.Email, User.Username, User.Password);
                User.Uid = Database.AddClientUser(User, Credentials);
                return new JsonResult(User);
            }
            return BadRequest(InvalidUserMessage);

        }
        //TODO: hash auth.
        [HttpPost]
        [Route("users/signin")]
        public IActionResult SignIn([FromBody] UserCredentials Credentials)
        {
            if(Auth.AuthUser(Credentials.Email, Credentials.Password))
            {
                return Ok();
            }
            return Unauthorized();
        }
        bool ValidUser(IAppUser User)
        {
            return ValidEmailPassword(User.Email, User.Password);
        }
        bool ValidEmailPassword(string Email, string Password)
        {
            return Validator.IsValidEmail(Email) && Validator.IsValidPassword(Password);
        }
    }
}