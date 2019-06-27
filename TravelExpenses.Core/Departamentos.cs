using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Departamentos
    {
        [Required, Key]
        public int IdDepto { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}