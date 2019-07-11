using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUbicacion
    {
        List<Estado> ObtenerEstados(string ClavePais);
        List<Paises> ObtenerPaises();
        List<Destino> ObtenerCiudades(int? IdEstado, string ClavePais);
    }
}
