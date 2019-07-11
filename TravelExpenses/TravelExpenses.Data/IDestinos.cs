using System.Collections.Generic;
using TravelExpenses.Core;
namespace TravelExpenses.Data
{
    public interface IDestinos
    {
        IEnumerable<Destinos> ObtenerDestinos(string Folio);
        Destinos ObtenerDestino(string Folio);
        int InsertarDestino(Destinos Destino);
    }
}
