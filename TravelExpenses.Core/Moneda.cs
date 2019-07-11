using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Moneda
    {
        public Moneda()
        {
            ClaveMoneda = string.Empty;
        }
        [Required, Key, StringLength(3)]
        public string ClaveMoneda { get; set; }
        [Required, StringLength(35)]
        public string Descripcion { get; set; }
        [StringLength(15)]
        public string NombreCorto { get; set; }
        [StringLength(3)]
        public string Simbolo { get; set; }
    }
}