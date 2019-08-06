using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class FormaPago
    {
        public FormaPago()
        {
            Id = "00";
            Descripcion = string.Empty;
            Activo = true;
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public bool Activo { get; set; }
    }
}