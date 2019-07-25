using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class Politica
    {
        public Politica()
        {
            IdPolitica = 0;
        }
        [Required, Key]
        public int IdPolitica { get; set; }
        [Required, StringLength(200)]
        public string Nombre { get; set; }
        public int IdGasto { get; set; }
        [Range(0.0, float.MaxValue)]
        public decimal ImportePermitido { get; set; }
        
        [StringLength(250)]
        public string MensajeError { get; set; }
        public bool Activo { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}