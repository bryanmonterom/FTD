using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public partial class FlowSteps
    {
        public int Id {get; set;}
        [Display(Name = "Asignado a:")]
        public int? IdEmployee {get; set; }
        [Required]
        [Display(Name = "Numero de Orden")]
        public int StepNumber { get; set; }
        [Display(Name = "Nombre del Paso")]
        public string StepName {get; set; }
        [Display(Name = "Tipo de Flujo")]
        public int IdFlowType { get; set; }
        [Display(Name = "Adjuntos?")]
        public bool IsAttachmentNeed { get; set; }
        [Display(Name = "Matriz de Aprobaciones?")]
        public bool IsMatrixNeeded { get; set; }
        [Display(Name = "Formulario de Transferencia")]
        public bool IsTransferNeed { get; set; }
        [Display(Name = "Aprobacion del Supervisor")]
        public bool IsSupervisorNeed { get; set; }
        [Display(Name = "Este paso bloquea la edicion para los Requisitores")]
        public bool IsBuyersBlocker { get; set; }
        [Display(Name = "Grupo de aprobadores")]
        public int? IdGroup { get; set; }
        public Employee Employee { get; set;}
        public FlowType FlowType {get;set;}
        public ICollection<Request> Requests { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
        public Group Group { get; set; }
        public AttachmentType AttachmentType { get; set; }
        [Display(Name = "Tipo de Adjunto (Si es requerido)")]
        public int? IdAttachmentType { get; set; }
        [Display(Name = "Grupo a Informar cuando se apruebe")]
        public int? IdGroupToInform { get; set; }
        public Group GroupToInform { get; set; }
        [Display(Name = "Este es el punto de restauracion cuando se rechace por documentacion?")]
        public bool IsRestorePoint { get; set; }
        [Display(Name = "Enviar Email cuando se apruebe?")]
        public bool SendEmailWhenCompleted { get; set; } = false;
        [Display(Name = "Este es el punto de inicio para los supervisores")]
        public bool IsBeginPointWhenSupervisor { get; set; }


    }
}