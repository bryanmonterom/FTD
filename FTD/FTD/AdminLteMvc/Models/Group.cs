using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Display(Name = "Grupo")]
        public string Name { get; set; }
        public ICollection<EmployeeGroups> EmployeeGroups { get; set; }
        public ICollection<FlowSteps> FlowSteps { get; set; }
        public ICollection<FlowSteps> FlowSteps1 { get; set; }


    }
}