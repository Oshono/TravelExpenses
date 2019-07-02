using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class EmpresaViewModel
    {
        public IEnumerable<Empresas> Empresas { get; set; }
        public Empresas Empresa { get; set; }
    }
}
