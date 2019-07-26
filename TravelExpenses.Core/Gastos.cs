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
            ClaveCentroCosto = string.Empty;
            ClaveProdServ = string.Empty;
        }
        [Required, Key]
        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        [RegularExpression("^[0-9]+$"), StringLength(20)]
        public string CuentaContable { get; set; }
        [StringLength(20)]
        public string ClaveCentroCosto { get; set; }
        public string ClaveProdServ { get; set;  }
        public bool Activo { get; set; }
    }
}