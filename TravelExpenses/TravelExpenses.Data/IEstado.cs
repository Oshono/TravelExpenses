using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IEstado
    {
        IEnumerable<Estado> GetEstados(string name);
        Estado GetEstado(int id);
        //Restaurant Update(Restaurant updatedRestaurant);
        //Restaurant Add(Restaurant newRestaurant);
        //Restaurant Delete(int id);
        int Commit();
    }
}
