using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class RequestType
    {
        public int Id { get; set; }
        [Display(Name = "Tipo de Solicitud")]
        public string Name { get; set; }
        public ICollection<Request> Requests { get; set; }

    }
}