using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Services
{
    public class Observaciones
    {
        private readonly ISolicitudes SolicitudesData;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IObservacionDA _ObservacionDA;
        private readonly IMoneda _MonedaData;
        public Observaciones(ISolicitudes solicitudes,
            IComprobante comprobante,
            IGasto gastos,
            IObservacionDA ObservacionDA,
            IMoneda MonedaData
            )
        {
            SolicitudesData = solicitudes;
            _comprobante = comprobante;
            _gastos = gastos;
            _ObservacionDA = ObservacionDA;
            this._MonedaData = MonedaData;
        }
        public IEnumerable<Solicitud> s(string IDs,string estatus, DateTime inicio, DateTime fin)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            string ID = IDs;
            if (estatus.Equals("PorLiberar"))
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", ID, inicio, fin);
                solicitud.Solicitudes = PorComprobar;
                return PorComprobar;
            }
            else if (estatus.Equals("Revisada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return PorAutorizar;

            }
            else if (estatus.Equals("PorComprobar"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorComprobar", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return PorAutorizar;

            }
            else if (estatus.Equals("Liberada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Liberada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return PorAutorizar;

            }
            else if (estatus.Equals("Todo"))
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", ID, inicio, fin);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", ID, inicio, fin);
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return solicitud.Solicitudes;
            }
            else if (estatus.Equals("Cerrada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return PorAutorizar;
            }
            else
            {
                var PorLiberar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", ID, inicio, fin);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", ID, inicio, fin);
                var result = PorLiberar.Union(PorAutorizar);
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorComprobar", ID, inicio, fin);
                var Liberada = _ObservacionDA.ObtenerSolicitudesXEstatus("Liberada", ID, inicio, fin);
                var result2 = PorComprobar.Union(Liberada);
                var Cerrada = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada", ID, inicio, fin);
                solicitud.Solicitudes = result.Union(result2).Union(Cerrada);
                return solicitud.Solicitudes;
            }
        }
    }
}
