using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminLteMvc.Models
{
    public partial class Status
    {
        [Display(Name = "Status")]
        public int Id { get; set; }
        [Display(Name="Estado")]
        public string Name { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set;}
        public ICollection<Request> Requests { get; set; }

    }
}