using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminLteMvc.Models
{
    public class Request
    {
        
        [Display(Name = "Numero de Orden")]
        public int Id { get; set; }
        [Display(Name = "Dirigo A:")]
        public int IdTo { get; set; }
        [Display(Name = "Desde:")]
        public int IdFrom { get; set; }
        [Display(Name = "Identificador")]
        public string Identifier { get; set; }
        [Display(Name = "Solicitante")]
        public int IdRequestor { get; set; }
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        [Display(Name = "Ultima modificacion")]
        public DateTime LastModified { get; set; }

        [Display(Name = "A:")]
        public Department To { get; set; }
        [Display(Name = "De:")]
        public Department From { get; set; }
        [Display(Name = "Asunto")]
        public string Subject { get; set; }
        [Display(Name = "Mensaje")]
        public string Message { get; set; }
        public Employee Requestor { get; set; }
        [Display(Name = "Estado")]
        public int IdStatus { get; set; }
        public Status Status { get; set; }
        public ICollection<Attachments> Attachments { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
        [Display(Name = "Tipo de Flujo")]
        public int IdFlowType { get; set; }
        [Display(Name = "Paso")]
        public int IdFlowSteps { get; set; }
        public FlowSteps FlowSteps { get; set; }
        public FlowType FlowType { get; set; }
        [Display(Name = "Completado")]
        public bool IsCompleted { get; set; }

    }
}