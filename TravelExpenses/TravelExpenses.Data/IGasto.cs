using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IGasto
    {
        IEnumerable<Gastos> ObtenerGastos();
        int Guardar(Gastos Gasto);
        int Commit();
    }
}
