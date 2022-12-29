using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
 
    public partial class FlowType
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tipo de Flujo")]
        public string Name { get; set; }
        public ICollection<FlowSteps> FlowSteps { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}