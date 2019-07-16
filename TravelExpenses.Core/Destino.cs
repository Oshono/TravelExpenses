using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Destino
    {    
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public int? IdEstado { get; set; }
        public string DescripcionEstado { get; set; }
        public string ClavePais { get; set; }
        public string NombrePais { get; set; }
        public bool Activo { get; set; }
    }
}
