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

        public int Folio { get; set; }
        public DateTime  FechaSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public double ImporteSolicitado { get; set; }
        public double ImporteComprobado { get; set; }

        public string ImporteSolicitadoC {
            get
            {
               return string.Format("{0:C}", this.ImporteSolicitado);
            }
        }
        public string ImporteComprobadoC {
            get { return string.Format("{0:C}", this.ImporteComprobado); }
        } 

        public string Estatus { get; set; }
        public int IdEstado { get; set; }
        public string Id { get; set; }
        public string RFC { get; set; }
        public string Descripcion { get; set; }
        public string UserName { get; set; }
        public string ClaveMoneda { get; set; }
        public int IdFolio { get; set; }
        public string MonedaNombre { get; set; }
        public string Comentarios { get; set; }
        public string CantidadComprobada { get; set; }
        public double ImporteExcedente { get; set; }
        public string ImporteExcedenteC { get { return string.Format("{0:C}", this.ImporteExcedente); } }
        public bool Exportar { get; set; }
        public bool ExportarRealizada { get; set; }

        public double Total { get; set; }
        public string TotalC { get { return string.Format("{0:C}", this.Total); } }

    }

    public class Politicas
    {

        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        public double ImportePermitido { get; set; }
        public string ClaveCentroCosto { get; set; }
        public int NumSolicitudes { get; set; }
        public int result { get; set; }
    }

    public class Usuarios
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Aprobador { get; set; }
    }
}