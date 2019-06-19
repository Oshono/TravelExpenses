using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUbicacion
    {
        List<Estado> ObtenerEstados();
    }
}
