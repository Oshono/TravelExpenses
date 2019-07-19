using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace TravelExpenses.Core
{
    public class Destinos
    {
        //Destinos

        [Required,Key]
        public int IdDestinos { get; set; }
        public string ClavePais { get; set; }
        public int IdEstado { get; set; }
        public int IdCiudad { get; set; }
        public string Motivo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        //public DateTime FechaSalida { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaLlegada { get; set; }

        //public DateTime FechaLlegada { get; set; }
        public int Folio{ get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        
    }
}
