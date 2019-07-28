using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IComprobante
    {
        IEnumerable<Comprobante> ObtenerComprobantes();
        List<Comprobante> ObtenerComprobantes(int FolioSolicitud);
        Comprobante ObtenerComprobantesXID(string UUID);
        int Guardar(Comprobante comprobante);
        int Commit();
    }
}
