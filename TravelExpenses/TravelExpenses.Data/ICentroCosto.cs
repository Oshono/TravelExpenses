using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ICentroCosto
    {
        IEnumerable<CentroCosto> ObtenerCentroCostos();
        int GuardarCentroConstosUsuario(CentroCostoUsuario centroCostoUsuario);
        IEnumerable<CentroCostoUsuario> ConsultarControCostoPorUsuario(string id);
        int Guardar(CentroCosto centroCosto);
        int Commit();
    }
}
