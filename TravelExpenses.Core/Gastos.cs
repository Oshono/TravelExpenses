using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Gastos
    {
        public Gastos()
        {
            IdGasto = 0;
            Activo = true;
        }
        [Required, Key]
        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        [RegularExpression("^[0-9]+$"), StringLength(20)]
        public string CuentaContable { get; set; }
        public bool Activo { get; set; }
    }
}