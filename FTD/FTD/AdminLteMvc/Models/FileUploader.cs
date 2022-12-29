using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class FileUploader
    {
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Seleccionar Archivo")]
        public HttpPostedFileBase files { get; set; }
        public int Id { get; set; }
        public int? IdFile { get; set; }
        [Display(Name = "Tipo de Adjunto")]
        public int IdAttachmentType { get; set; }


    }
}