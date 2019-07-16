using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IReembolso
    {
        int Guardar(Archivo archivo);
        IEnumerable<Archivo> ObtenerArchivos();
    }
}
