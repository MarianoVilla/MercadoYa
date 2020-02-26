using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoYa.Rest.Logic
{
    public static class ExtensionMethods
    {
        public static async Task<T> GetFromBody<T>(this HttpRequest Request)
        {
            Request.EnableBuffering();
            Stream req = Request.Body;
            req.Seek(0, SeekOrigin.Begin);
            string json = await new StreamReader(req).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
