using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DatabaseController(ILogger<DatabaseController> logger)
        {
            this.Logger = logger;
        }


    }
}