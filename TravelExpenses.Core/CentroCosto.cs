using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class CentroCosto
    {
        public CentroCosto()
        {
            ClaveCentroCosto = string.Empty;
            Activo = true;
        }
        [Required, Key]
        public string ClaveCentroCosto { get; set; }
        [Required]
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}