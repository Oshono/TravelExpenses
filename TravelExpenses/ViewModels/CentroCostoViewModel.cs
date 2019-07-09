using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class CentroCostoViewModel
    {
        public IEnumerable<CentroCosto> CentrosCostos { get; set; }
        public IEnumerable<Empresas> Empresas { get; set; }
        public CentroCosto CentroCosto { get; set; }
    }
}
