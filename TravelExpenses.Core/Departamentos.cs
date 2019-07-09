using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Departamentos
    {
        public Departamentos()
        {
            ClaveDepto = string.Empty;
            Nombre = string.Empty;
        }
        [Required, Key, StringLength(10)]
        public string ClaveDepto { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}