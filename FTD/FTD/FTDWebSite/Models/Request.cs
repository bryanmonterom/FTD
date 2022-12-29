using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTDWebSite.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdRequestType { get; set; }
    }
}