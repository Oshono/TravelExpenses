using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class ReembolsoViewModel
    {
        public IEnumerable<Archivo> Archivos { get; set; }
        public Archivo miArchivo { get; set; }
    }
}
