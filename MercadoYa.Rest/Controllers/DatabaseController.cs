using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
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
        public IActionResult GetUserInfo(string Uid)
        {
            if (IsUnauthorized())
                return Unauthorized();

            IAppUser User = Database.GetUser(Uid);
            return new JsonResult(User);
        }
        [Route("nearbystores")]
        [HttpPost]
        public IActionResult GetNearbyStores([FromBody] LocationRequest LocRequest)
        {
            if (IsUnauthorized())
                return Unauthorized();

            IEnumerable<IAppUser> Stores = Database.GetNearbyStores(LocRequest);
            return new JsonResult(Stores);
        }


        bool IsUnauthorized()
        {
            var Timestamp = Request.Cookies["timestamp"] as string;
            return !ValidTimestamp(Timestamp);
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