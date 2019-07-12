using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;
namespace TravelExpenses.ViewModels
{
    public class DestinosViewModel
    {
        public IEnumerable<Destinos> Destinos { get; set; }
        public Destinos Destino { get; set; }
        public string ClavePais { get; set; }
        public int IdEstado { get; set; }
    }
}
