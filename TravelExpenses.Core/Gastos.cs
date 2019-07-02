using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Gastos
    {
        [Required, Key]
        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}