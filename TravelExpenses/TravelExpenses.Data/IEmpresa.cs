using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IEmpresa
    {
        List<Empresas> ObtenerEmpresas(string RFC);
        int Guardar(Empresas Empresa);
        Empresas Add(Empresas newRestaurant);
    }
}