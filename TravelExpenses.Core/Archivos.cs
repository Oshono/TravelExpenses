using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Archivo
    {    
        [Key]
        public int IdArchivo { get; set; }
        [Required]
        public string NombreArchivo { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string Ruta { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Usuario { get; set; }
        public string Estatus { get; set; }
    }
}
