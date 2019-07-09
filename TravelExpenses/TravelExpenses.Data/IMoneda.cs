using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IMoneda
    {
        IEnumerable<Moneda> ObtenerMonedas();
        int Guardar(Moneda centroCosto);
        int Commit();
    }
}
