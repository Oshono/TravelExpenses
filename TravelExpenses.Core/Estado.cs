using System;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Estado
    {
        [Required, Key]
        public int IdEstado { get; set; }

        [Required, StringLength(50)]
        public string NombreEstado { get; set; }

        public Paises Pais { get; set; }
        public bool Activo { get; set; }

    }
}
