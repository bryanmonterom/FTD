using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class AttachmentType
    {
        public int Id { get; set; }
        [Display(Name = "Tipo de Adjunto")]
        public string Name { get; set; }
        public ICollection<Attachments> Attachments { get; set; }
        public ICollection<FlowSteps> FlowSteps { get; set; }
    }
}