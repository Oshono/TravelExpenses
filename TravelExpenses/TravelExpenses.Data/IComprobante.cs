using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IComprobante
    {
        IEnumerable<Comprobante> ObtenerComprobantes();
        int Guardar(Comprobante comprobante);
        int Commit();
    }
}
