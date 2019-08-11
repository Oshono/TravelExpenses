using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class Comprobante
    {
        [Key]
        public string UUID { get; set; }
        [Required]
        public string Folio { get; set; }
        [Required]
        public string RFC { get; set; }
        public string NombreProveedor { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        
        public double SubTotal { get; set; }
        public double Impuestos { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "float")]
        public double Retenciones { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "float")]
        public double Total { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "float")]
        [NotMapped]
        public double ImporteComprobado { get; set; }
        public string RegimenFiscal { get; set; }
        public string Moneda { get; set; }
        public string FormaPago { get; set; }
        public int FolioSolicitud { get; set; }
        public List<Concepto> Conceptos { get; set; }        
        public List<Archivo> Archivos { get; set; }
        [NotMapped]
        public bool ComprobanteXML { get; set; }
        [NotMapped]
        public string MensajeError { get; set; }
    }
}