using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class ReasonToReject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RequestHistory> RequestHistores { get; set; }
    }
}