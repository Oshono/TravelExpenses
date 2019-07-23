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
 
        IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus);
        int EliminarSolicitud(int Folio);
        int InsertarGastos(Gasto gasto);
        IEnumerable<Gasto> ObtenerGastos(int folio);
    }
}
