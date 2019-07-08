using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class GastoViewModel
    {
        public IEnumerable<Gastos> Gastos { get; set; }
        public Gastos Gasto { get; set; }
    }
}
