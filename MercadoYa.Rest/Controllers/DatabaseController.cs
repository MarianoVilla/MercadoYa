using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Rest.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MercadoYa.Rest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        readonly ILogger<DatabaseController> Logger;
        readonly IDatabase Database;
        public DatabaseController(ILogger<DatabaseController> logger, IDatabase Database)
        {
            this.Logger = logger;
            this.Database = Database;
        }
        [Route("test")]
        public IActionResult Test() 
        { 
            return Ok(); 
        }
        [Route("users")]
        public IActionResult GetUserInfo([FromBody] string Uid)
        {
            var Timestamp = Request.Cookies["timestamp"] as string;
            if(!ValidTimestamp(Timestamp))
            {
                return Unauthorized();
            }
            IAppUser User = Database.GetUser(Uid);
            return new JsonResult(User);
        }
        bool ValidTimestamp(string Timestamp)
        {
            if (Timestamp is null)
                return false;
            Timestamp = CryptoUtil.DecryptString(Const.CryptoDevKey, Timestamp);
            if (!DateTime.TryParseExact(Timestamp, "yyyyMMddHHmmssFFF", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime Result))
                return false;
            return (DateTime.Now - Result).TotalDays < 1;
        }

    }
}