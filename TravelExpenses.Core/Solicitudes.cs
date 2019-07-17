using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Solicitud
    {

        public enum EstatusSolicitud
        {
            Cancelada = 1,
            PorComprobar=2,
            SolicitudRechazada=3,
            SolicitudCapturada=4,
            SolicitudporAutorizar=5,
            PendienteAutorizar=6,
            PorLiberar=7
        } 

        //public int IdSolicitud { get; set; }

        public string Folio { get; set; }
        public DateTime  FechaSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public double ImporteSolicitado { get; set; }
        public double ImporteComprobado { get; set; }
        public string Estatus { get; set; }
        public int IdEstado { get; set; }
        public string Id { get; set; }
        public string RFC { get; set; }
        public string Descripcion { get; set; }
        public string UserName { get; set; }

    }
}