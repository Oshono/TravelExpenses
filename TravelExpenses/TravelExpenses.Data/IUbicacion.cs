using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUbicacion
    {
        List<Estado> ObtenerEstados(string ClavePais);
        List<Paises> ObtenerPaises();
        List<Ciudades> ObtenerCiudades(string ClavePais, int? IdEstado);
        List<Destino> ObtenerDestinos(int? IdEstado, string ClavePais);
        int GuardarCiudad(Ciudades ciudad);
        int GuardarEstado(Estado estado);
        int Commit();
        //IEnumerable<Paises> ObtenerPais();
        //IEnumerable<Estado> ObtenerEstado(string ClavePais);
        //IEnumerable<Ciudades> ObtenerCiudad(string Clave, int IdEstado);
    }
}
