using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IComprobante
    {
        IEnumerable<Comprobante> ObtenerComprobantes();
        IEnumerable<Archivo> ObtenerArchivosXNombre(string Nombre);
        List<Comprobante> ObtenerComprobantes(int FolioSolicitud);
        Comprobante ObtenerComprobantesXID(string UUID);
        int Guardar(Comprobante comprobante);
        void Delete(string UUID);
        int Commit();
    }
}
