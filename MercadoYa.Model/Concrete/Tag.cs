using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class Tag : ITag
    {
        public string TagName { get; set; }
        public string TagDescription { get; set; }
        public int Id { get; set; }
    }
}
