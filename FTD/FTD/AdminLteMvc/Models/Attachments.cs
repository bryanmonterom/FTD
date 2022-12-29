using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class Attachments
    {
        public int Id { get; set; }
        [Display(Name = "Nombre de Archivo")]
        public string Name { get; set; }
        [Display(Name = "ID Solicitud")]
        public int IdRequest { get; set; }
        public byte[] ByteContent { get; set; }
        [Display(Name = "Tipo de Archivo")]
        public string ContentType { get; set; }
        [Display(Name = "ID Tipo de Adjunto")]
        public Request Request { get; set; }
        public int? IdAttachmentType { get; set; }
        public byte[] Vector { get; set; }
        public AttachmentType AttachmentType { get; set; }
    }
}