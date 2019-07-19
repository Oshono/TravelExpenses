using System.Collections.Generic;
using TravelExpenses.Core;
namespace TravelExpenses.Data
{
    public interface IDestinos
    {
        IEnumerable<Destinos> ObtenerDestinos(int Folio);
        Destinos ObtenerDestino(int Folio);
        int InsertarDestino(Destinos Destino);
    }
}
