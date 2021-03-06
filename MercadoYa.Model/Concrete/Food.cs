﻿using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class Food : ITag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string TagDescription { get; set; }
        public string Region { get; set; }
    }
}
