//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTIService
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestHistories
    {
        public int Id { get; set; }
        public int IdRequest { get; set; }
        public System.DateTime DateApproval { get; set; }
        public int IdEmployee { get; set; }
        public string Comments { get; set; }
        public int IdStatus { get; set; }
        public int IdFlowSteps { get; set; }
        public Nullable<int> IdRejectReasons { get; set; }
    
        public virtual Employees Employees { get; set; }
        public virtual FlowSteps FlowSteps { get; set; }
        public virtual ReasonToRejects ReasonToRejects { get; set; }
        public virtual Requests Requests { get; set; }
        public virtual Status Status { get; set; }
    }
}
