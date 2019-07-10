using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class CentroCosto
    {
        public CentroCosto()
        {
            ClaveCentroCosto = string.Empty;
            Activo = true;
        }
        [Required, Key, StringLength(20)]
        public string ClaveCentroCosto { get; set; }
        [StringLength (50)]
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        [NotMapped]
        public bool checkboxAnswer { get; set; }
    }
}