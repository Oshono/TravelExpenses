using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
        public string Fecha { get; set; }
        public float SubTotal { get; set; }
        public float Impuestos { get; set; }
        public float Retenciones { get; set; }
        public float Total { get; set; }
        public string RegimenFiscal { get; set; }
        public string Moneda { get; set; }
        
        public List<Concepto> Conceptos { get; set; }
        public Archivo Archivo { get; set; }
    }
}