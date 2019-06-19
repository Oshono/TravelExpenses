using System;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Ciudades
    {
        [Required, Key]
        public int IdCiudad { get; set; }
        [Required]
        public string Ciudad { get; set; }
        public Estado Estado { get; set; }
        public bool Activo { get; set; }

    }
}
