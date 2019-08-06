using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class Politica
    {
        public Politica()
        {
            IdPolitica = 0;
            NumSolicitudes = 0;
            ImportePermitido = 0;
        }
        [Required, Key]
        public int IdPolitica { get; set; }
        [Required, StringLength(200)]
        public string Nombre { get; set; }
        [NotMapped]
        public int IdGasto { get; set; }
        [NotMapped]
        [Range(0.0, float.MaxValue)]
        public decimal ImportePermitido { get; set; }
        [StringLength(20)]
        public string ClaveCentroCosto { get; set; }
        [StringLength(250)]
        public string MensajeError { get; set; }
        public bool Activo { get; set; }
        public byte NumSolicitudes { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public List<PoliticaDetalle> Detalle { get; set; }
    }
}