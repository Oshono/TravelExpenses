using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IPolitica
    {
        IEnumerable<Politica> ObtenerPoliticas();
        int Guardar(Politica politica);
        int Commit();       
    }
}
