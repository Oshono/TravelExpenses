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
        IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus);
        //List<Destinos> ObtenerDestinos(int IdDestinos);
        //int Guardar(Solicitud solicitud);
        //Solicitud Add(Solicitud solicitud);
    }
}
