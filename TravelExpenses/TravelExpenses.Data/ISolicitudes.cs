using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ISolicitudes
    {
        int InsertarSolicitud(Solicitud solicitud);
        int InsertarDestino(Destinos Destino);
        Solicitud ObtenerTipo();
        IEnumerable<Solicitud> ObtenerTipos();
        IEnumerable<Solicitud> ObtenerTipoSolicitud();
        IEnumerable<Solicitud> ObtenerSolicitudes();
         
        IEnumerable<Solicitud> ObtenerIdSolicitud();
        IEnumerable<Solicitud> ObtenerSolicitudesEstatus(string estatus);
        int ModificarEstatus(int Folio);
        int ActualizarEstatus(int Folio, string Estatus);
        IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus);
        int EliminarSolicitud(int Folio);
        int InsertarGastos(Gasto gasto);
        //IEnumerable<Gasto> _ObtenerGastos(int folio);
        IEnumerable<Estatus> EstatusSolicitudes();
        Solicitud SolicitudesXFolio(int Folio);
        IEnumerable<Gasto> ObtenerGastos(int Folio);
        IEnumerable<Destinos> DestinosXFolio(int Folio);
        int Destinos_Upd(Destinos _destinos);
        Destinos ObtenerDestinosFolio(int IdDestinos);
        int Gastos_Upd(Gasto _Gastos);
        Gasto ObtenerGastosFolio(int IdGastos);
        int EliminarDestinos(int IdDestinos);
        int EliminarGastos(int IdGastos);
        int SolicitudesUpd(Solicitud solicitud);
    }
}
