using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class DestinoViewModel
    {
        public IEnumerable<Destino> Destinos { get; set; }
        public IEnumerable<Ciudades> Ciudades { get; set; }
        public IEnumerable<Estado> Estados { get; set; }
        public IEnumerable<Paises> Paises { get; set; }

        public Estado Estado { get; set; }
        public Paises Pais { get; set; }
        public Ciudades Ciudad { get; set; }

    }
}
