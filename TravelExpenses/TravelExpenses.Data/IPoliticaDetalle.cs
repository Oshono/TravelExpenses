using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IPoliticaDetalle
    {
        List<PoliticaDetalle> ObtenerDetalles(int IdPolitica);
        List<PoliticaDetalle> ObtenerDetalles();
        int Guardar(PoliticaDetalle detalles);
        int Commit();       
    }
}
