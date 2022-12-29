using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Display(Name = "Departamento")]
        public string Name { get; set; }
        [Display(Name = "Identificador")]
        public string Identifier { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<Request> Requests2 { get; set; }

        public ICollection<Employee> Employees { get; set; }
        [Display(Name = "Director")]
        public int? IdDirector { get; set; }

    }
}