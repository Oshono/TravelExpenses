using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IRembolso
    {
        int Guardar(Comprobante comprobante);  
        int GuardarComprobante(Comprobante miComprobante);
        IEnumerable<Archivo> ObtenerArchivos();
        bool Exists(string NombreArchivo, string Extension);
    }
}
