using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class RejectReasons
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RequestHistory> PostHistories { get; set; }
    }
}