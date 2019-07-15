using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ICentroCostoEmpresa
    {
        IEnumerable<CentroCostoEmpresa> ObtenerCentroCostosEmpresa();
        IEnumerable<Empresas> ObtenerEmpresas();
        IEnumerable<CentroCosto> ObtenerCentrosCostos();
        int Guardar(CentroCostoEmpresa centroCostoEmpresa);
        int Borrar(CentroCostoEmpresa centroEmpresa);
        int Commit();
    }
}
