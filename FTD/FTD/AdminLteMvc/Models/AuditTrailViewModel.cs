using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class AuditTrailViewModel
    {
        [Display(Name = "Realizado por")]
        public string PerformedBy { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Accion")]
        public string Action { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Hasta:")]

        public DateTime? ToDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Desde:")]

        public DateTime? FromDate { get; set; }

    }
}