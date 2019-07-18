using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Ciudades
    {
        [Required, Key]
        public int IdCiudad { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public int? IdEstado { get; set; }
        public string ClavePais { get; set; }
        public bool Activo { get; set; }
    }
}
