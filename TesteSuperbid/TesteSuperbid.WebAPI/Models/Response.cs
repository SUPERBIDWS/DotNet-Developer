using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteSuperbid.WebAPI.Models
{
    public class Response
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public object Obj { get; set; }
    }
}