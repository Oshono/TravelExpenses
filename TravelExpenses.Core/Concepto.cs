using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class Concepto
    {
        public Concepto()
        {
            IdGasto = 0;
            Cantidad = 0;
            Importe = 0;
            Descripcion = string.Empty;
            TasaOCuota = 0;
            Impuesto = 0;
            Base = 0;
            IdGasto = 0;
        }
        [Key]
        public int IdConcepto { get; set; }
        [Required]
        public string UUID { get; set; }
        [Required]
        public double Importe { get; set; }
        [Required]
        public double ValorUnitario { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Unidad { get; set; }
        public string ClaveUnidad { get; set; }
        public double Cantidad { get; set; }
        public string NoIdentificacion { get; set; }
        public string ClaveProdServ { get; set; }        
        public double TasaOCuota { get; set; }
        public string TipoFactor { get; set; }
        public double Impuesto { get; set; }
        public double Base { get; set; }
        public int IdGasto { get; set; }
        
        public string DescripcionProdServ { get; set; }
        [NotMapped]
        public string MensajeError { get; set; }
    }
}