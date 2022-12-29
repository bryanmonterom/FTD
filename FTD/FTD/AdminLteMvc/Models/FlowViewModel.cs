using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class FlowViewModel
    {
        [Display(Name = "Paso")]
        public string StepName { get; set; }
        public Status Status { get; set; }
        public Employee Employee { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}