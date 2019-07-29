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
        public int IdTipoSolicitud { get; set; }

        public IEnumerable<Estatus> Estatuses { get; set; }
        public Estatus Estatus { get; set; }
        public int IdEstatus { get; set; }

        public IEnumerable<Destinos> Destinos { get; set; }
        public Destinos Destino { get; set; }

        public IEnumerable<Paises> Paises { get; set; }
        public Paises Pais { get; set; }
        public string ClavePais { get; set; }

        public IEnumerable<Estado> Estados { get; set; }
        public Estado Estado { get; set; }
        public int IdEstado { get; set; }

        public IEnumerable<Ciudades> Ciudades { get; set; }
        public Ciudades Ciudad { get; set; }

        public IEnumerable<Moneda> Monedas { get; set; }
        public Moneda Moneda { get; set; }
        public string ClaveMoneda { get; set; }

        public IEnumerable<Gasto> Gastos { get; set; }
        public Gasto Gasto { get; set; }
        public int IdGasto { get; set; }

        public IEnumerable<Empresas> Empresas { get; set; }
        public Empresas Empresa{ get; set; }
        public string RFC { get; set; }

        public IEnumerable<Gastos> _Gastos { get; set; }
        public Gastos _Gasto { get; set; }
    }
}
