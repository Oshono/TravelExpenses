using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ICentroCostoEmpresa
    {
        IEnumerable<CentroCostoEmpresa> ObtenerCentroCostosEmpresa();
        int Guardar(CentroCostoEmpresa centroCostoEmpresa);
        int Commit();
    }
}
