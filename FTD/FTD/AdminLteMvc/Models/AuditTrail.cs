using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public partial class AuditTrail
    {
        public int Id { get; set; }
        [Display(Name = "Realizado por")]
        public string PerformedBy { get; set; }
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        [Display(Name = "Accion")]
        public string Action { get; set; }
        [Display(Name = "Detalles")]
        public string ActionMessage { get; set; }

    }
}