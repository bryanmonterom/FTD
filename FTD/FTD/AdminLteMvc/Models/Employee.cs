using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Usuario de Red")]
        public string Identification { get; set; }
        [Required]
        [Display(Name = "Nombre de Empleado")]
        public string Name { get; set; }
        [Required]
        [DataType(dataType:DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Supervisor")]
        public int? IdSupervisor { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
        public ICollection<Request> Request { get; set; }
        public ICollection<FlowSteps> FlowSteps { get; set; }
        public ICollection<Employee> Supervised { get; set; }
        public Employee Employees { get; set; }
        public ICollection<EmployeeGroups> EmployeeGroups { get; set; }
        [Display(Name = "Departamento")]
        public int? IdDepartment { get; set; }
        public Department Department { get; set; }
        [Display(Name = "¿Es Supervisor?")]
        public bool IsSupervisor { get; set; }

        //----
        public string FileName { get; set; }
        [Display(Name = "Imagen de Firma")]
        public byte[] ByteContent { get; set; } = null;
        public string ContentType { get; set; }


    }
}