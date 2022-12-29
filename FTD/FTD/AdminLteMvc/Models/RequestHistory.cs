using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace AdminLteMvc.Models
{
    public partial class RequestHistory
    {
        public int Id { get; set; }
        [Display(Name = "Identificador")]
        public int IdRequest { get; set; }
        [Display(Name ="Ultima modificacion")]
        public DateTime DateApproval { get; set; }
        [Display(Name = "Aprobador")]
        public int? IdEmployee { get; set; }
        [Display(Name = "Comentarios")]
        [Required]
        public string Comments { get; set; }
        [Display(Name = "Estado")]
        public int IdStatus { get; set; }
        [Display(Name = "Paso")]
        public int IdFlowSteps { get; set; }
        public Request Request { get; set; }
        public Employee Employee { get; set; }
        [Display(Name = "Razon del Rechazo")]
        public int? IdRejectReasons { get; set; }
        public Status Status { get; set; }
        public FlowSteps FlowSteps { get; set; }
        public ReasonToReject RejectReasons { get; set; }


    }
}