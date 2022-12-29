using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class RequestReportViewModel
    {
        [Display(Name = "ID Solicitud ej: 45")]
        public int PostId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Creados Desde:")]
        public DateTime ToDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Hasta:")]
        public DateTime FromDate { get; set; }

    }
}