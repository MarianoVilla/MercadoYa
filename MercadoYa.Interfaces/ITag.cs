using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface ITag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string TagDescription { get; set; }
    }
}
