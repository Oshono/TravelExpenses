using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IPoliticaDetalle
    {
        List<PoliticaDetalle> ObtenerDetalles(int IdPolitica);
        int Guardar(PoliticaDetalle detalles);
        int Commit();       
    }
}
