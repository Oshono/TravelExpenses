using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IEmpresa
    {
        IEnumerable<Empresas> ObtenerEmpresas();
        Empresas ObtenerEmpresa(string RFC);
        int Guardar(Empresas Empresa);
        Empresas Add(Empresas newRestaurant);
    }
}