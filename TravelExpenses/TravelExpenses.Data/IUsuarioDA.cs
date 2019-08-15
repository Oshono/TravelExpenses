using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IUsuario
    {
        CorreosDestinatarios ObtenerCorreos(string username, string centroCosto, int? Folio);
        int Commit();       
    }
}
