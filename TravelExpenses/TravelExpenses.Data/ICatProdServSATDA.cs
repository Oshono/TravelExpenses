using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface ICatProdServSATDA
    {
        IEnumerable<CatProdServSAT> ObtenerCatalogo();
        
    }
}
