using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class DepartamentoViewModel
    {
        public IEnumerable<Departamentos> Departamentos { get; set; }
        public Departamentos Departamento { get; set; }
    }
}
