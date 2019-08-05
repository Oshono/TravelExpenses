using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class PoliticaDetalle
    {
        public PoliticaDetalle()
        {
            ImportePermitido = 0;
            Activo = true;
        }
        [Key,Required]
        public int IdPolitica { get; set; }
        [Key, Required]
        public int IdGasto { get; set; }

        [NotMapped]
        public string DescripcionGasto { get; set; }
        [Range(0.0, float.MaxValue)]
        public decimal ImportePermitido { get; set; }
        public bool Activo { get; set; }
    }
}