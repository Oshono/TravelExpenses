using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelExpenses.Core
{
    public class Observacion
    {
        public Observacion()
        {
            FechaAlta = DateTime.Now;
        }
        [Required, Key]
        public int Folio { get; set; }
        
        public string Descripcion { get; set; }

        public DateTime FechaAlta { get; set; }

    }
}
