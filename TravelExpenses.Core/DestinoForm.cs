using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelExpenses.Core
{
    public class DestinoForm
    {

        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaLlegada { get; set; }
        public string Motivo { get; set; }

        public string ClavePais { get; set; }
        public int IdEstado { get; set; }
        public int IdCiudad { get; set; }
        public int Folio { get; set; }



    }
}
