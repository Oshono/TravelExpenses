using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IGasto
    {
        IEnumerable<Gastos> ObtenerGastos();
        IEnumerable<Gastos> ObtenerGastosPoliticas(string IDUser);
        int Guardar(Gastos Gasto);
        int Commit();
    }
}
