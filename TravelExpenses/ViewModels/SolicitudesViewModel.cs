using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class SolicitudesViewModel
    {
        public IEnumerable<Solicitud> Solicitudes { get; set; }
        public Solicitud Solicitud { get; set; }

        public IEnumerable<Destinos> Destinos { get; set; }
        public Destinos Destino { get; set; }

        public IEnumerable<Paises> Paises { get; set; }
        public Paises Pais { get; set; }
 
        public IEnumerable<Estado> Estados{ get; set; }
        public Estado Estado{ get; set; }

        public IEnumerable<Ciudades> Ciudades{ get; set; }
        public Ciudades Ciudad { get; set; }
    }
}
