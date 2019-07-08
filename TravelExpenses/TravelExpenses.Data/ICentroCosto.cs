using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ICentroCosto
    {
        IEnumerable<CentroCosto> ObtenerCentroCostos(string RFC);
        int Guardar(CentroCosto centroCosto);
        int Commit();
    }
}
