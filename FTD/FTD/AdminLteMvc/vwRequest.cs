//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminLteMvc
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwRequest
    {
        public int Id { get; set; }
        public int IdTo { get; set; }
        public int IdFrom { get; set; }
        public string Identifier { get; set; }
        public int IdRequestor { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime LastModified { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int IdStatus { get; set; }
        public int IdFlowType { get; set; }
        public int IdFlowSteps { get; set; }
        public bool IsCompleted { get; set; }
        public string departmentFrom { get; set; }
        public string departmentTo { get; set; }
        public string requestor { get; set; }
        public string status { get; set; }
        public string flowtype { get; set; }
        public string step { get; set; }
        public string dFromIdentifier { get; set; }
        public string directorFrom { get; set; }
        public string directorTo { get; set; }
        public byte[] fromFirma { get; set; }
        public byte[] toFirma { get; set; }
    }
}
