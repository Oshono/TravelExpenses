using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.TravelExpenses.Data
{
    public interface IObservacionDA
    {
        Observacion ObtenerObservacion(int Folio);
        Observacion Add(Observacion newRestaurant);
        int Guardar(Observacion Observacion);
        IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus, string ID);
    }
}
