using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class MonedaViewModel
    {
        public IEnumerable<Moneda> Monedas { get; set; }
        public Moneda moneda { get; set; }
    }
}
