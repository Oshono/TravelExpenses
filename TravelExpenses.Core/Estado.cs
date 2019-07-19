using System;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Estado
    {
        [Required, Key]
        public int IdEstado { get; set; }

        [Required, StringLength(50)]
        public string Descripcion { get; set; }

        [Required, StringLength(50)]
        public string ClavePais { get; set; }
        public bool Activo { get; set; }

    }
}
