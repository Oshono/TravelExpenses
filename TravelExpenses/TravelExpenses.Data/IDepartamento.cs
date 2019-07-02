using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IDepartamento
    {
        IEnumerable<Departamentos> ObtenerDepartamentos();
        int Guardar(Departamentos Depto);
        int Commit();
    }
}
