using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Core
{
    public class DestinoForm
    {

        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public string Motivo { get; set; }

        public string ClavePais { get; set; }
        public int IdEstado { get; set; }
        public int IdCiudad { get; set; }
        public int Folio { get; set; }



    }
}
