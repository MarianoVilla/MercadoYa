﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MercadoYa.Model
{
    public class Const
    {
        public static readonly Uri RestUri = new Uri("http://f72547ae.ngrok.io/");
        public static HttpClient GlobalHttpClient;
    }
}
