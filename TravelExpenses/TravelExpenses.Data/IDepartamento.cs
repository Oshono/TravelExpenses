using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IDepartamento
    {
        IEnumerable<Departamentos> ObtenerDepartamentos();
        Departamentos Add(Departamentos newDepartamento);
        //Restaurant Update(Restaurant updatedRestaurant);
        //Restaurant Add(Restaurant newRestaurant);
        //Restaurant Delete(int id);
        int Commit();
    }
}
