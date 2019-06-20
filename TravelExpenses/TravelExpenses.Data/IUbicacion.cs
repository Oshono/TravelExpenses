using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUbicacion
    {
        List<Estado> ObtenerEstados();
        List<Ciudades> ObtenerCiudades(int? IdEstado);
    }
}
