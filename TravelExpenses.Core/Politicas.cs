using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Politica
    {
        public Politica()
        {
            ClavePolitica = string.Empty;
        }
        [Required, Key]
        public string ClavePolitica { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public int IdGasto { get; set; }
        public float Limite { get; set; }
        public string UserName { get; set; }
        public string MensajeError { get; set; }
        public bool Activo { get; set; }
    }
}