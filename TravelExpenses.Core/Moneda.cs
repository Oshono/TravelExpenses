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
        [Required, Key]
        public string ClaveMoneda { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string NombreCorto { get; set; }
        public string Simbolo { get; set; }
    }
}