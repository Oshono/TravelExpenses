using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace TravelExpenses.Core
{
    public class Solicitud
    {
        public string Folio { get; set; }
        public string TipoSolicitud { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public double ImporteSolicitado { get; set; }
        public double ImporteComprobado { get; set; }
        public string Estatus { get; set; }
        public string Motivo { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuario { get; set; }
    } 
}