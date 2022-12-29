using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class EmployeeGroups
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Empleado")]
        public int IdEmployee { get; set; }
        [Display(Name = "Grupo")]
        public int IdGroup { get; set; }
        public Employee Employee { get; set; }
        public Group Group { get; set; }
    }
}