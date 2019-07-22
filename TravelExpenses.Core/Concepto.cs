using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Concepto
    {
        [Key]
        public int IdConcepto { get; set; }
        [Required]
        public string UUID { get; set; }
        [Required]
        public float Importe { get; set; }
        [Required]
        public float ValorUnitario { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Unidad { get; set; }
        public string ClaveUnidad { get; set; }
        public float Cantidad { get; set; }
        public string NoIdentificacion { get; set; }
        public string ClaveProdServ { get; set; }
        public float TasaOCuota { get; set; }
        public string TipoFactor { get; set; }
        public float Impuesto { get; set; }
        public float Base { get; set; }
    }
}