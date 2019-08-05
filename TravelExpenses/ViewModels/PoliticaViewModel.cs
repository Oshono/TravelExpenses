using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class PoliticaViewModel
    {
        public IEnumerable<Politica> Politicas { get; set; }
        public Politica Politica { get; set; }
        public IEnumerable<Gastos> Gastos { get; set; }
        public IEnumerable<CentroCosto> CentroCostos { get; set; }
    }
}
