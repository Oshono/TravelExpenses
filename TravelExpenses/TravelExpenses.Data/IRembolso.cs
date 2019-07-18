using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IRembolso
    {
        int Guardar(Archivo archivo);
        IEnumerable<Archivo> ObtenerArchivos();
    }
}
