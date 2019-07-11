using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUbicacion
    {
        List<Estado> ObtenerEstados(string ClavePais);
        List<Paises> ObtenerPaises();
        List<Ciudades> ObtenerCiudades(int? IdEstado);
        IEnumerable<Paises> ObtenerPais();
        IEnumerable<Estado> ObtenerEstado(string ClavePais);
        IEnumerable<Ciudades> ObtenerCiudad(string Clave, int IdEstado);
    }
}
