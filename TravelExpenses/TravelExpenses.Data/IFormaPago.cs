using System.Collections.Generic;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public interface IFormaPago
    {
        IEnumerable<FormaPago> ObtenerFormasPago();        
    }
}
