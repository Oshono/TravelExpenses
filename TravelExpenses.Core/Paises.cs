using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Paises
    {
        [Required, Key]
        public string ClavePais { get; set; }
        public string Nombre { get; set; }
    }
}
