using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.TravelExpenses.Data;
using TravelExpenses.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelExpenses.Controllers
{
    public class AprobadorController : Controller
    {
        private readonly ISolicitudes SolicitudesData;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IObservacionDA _ObservacionDA;
        private readonly IMoneda _MonedaData;
        public AprobadorController(ISolicitudes solicitudes,
            IComprobante comprobante,
            IGasto gastos,
            IObservacionDA ObservacionDA,
            IMoneda MonedaData)
        {
            SolicitudesData = solicitudes;
            _comprobante = comprobante;
            _gastos = gastos;
            _ObservacionDA = ObservacionDA;
            this._MonedaData = MonedaData;
        }


        // GET: /<controller>/

        public ActionResult AprobarSolicitud()
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("Comprobada");
            var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("PorAutorizar");
            solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
            var Estatus = 
                    (from e in SolicitudesData.EstatusSolicitudes()
                    where e.Status == "Comprobada" | e.Status == "PorAutorizar"
                           select e
                        );
            solicitud.Estatuses = Estatus;
            return View(solicitud);
        }
 
        public IActionResult SolicitudesEstatus(string estatus)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            if (estatus.Equals("Comprobada"))
            {
                var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("Comprobada");
                solicitud.Solicitudes = PorComprobar;
                return Json(PorComprobar);
            }
            else if (estatus.Equals("PorAutorizar"))
            {
                var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("PorAutorizar");
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else
            {
                var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("Comprobada");
                var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("PorAutorizar");
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return Json(solicitud.Solicitudes);
            }

        }



        public ActionResult Aprobar(string Folio)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            solicitud.Solicitudes = SolicitudesData.ObtenerSolicitudes();

            return View("AprobarSolicitud",solicitud);
        }
        public ActionResult Detalles(string Folio)
        {
            var rembolso = new ObservacionViewModel();
            try
            {
                int FolioSolicitud = 0;
                
                if (int.TryParse(Folio, out FolioSolicitud))
                {
                    rembolso.Comprobantes = new List<Comprobante>();
                    rembolso.Comprobantes = _comprobante.ObtenerComprobantes(FolioSolicitud);
                    var solicitud = SolicitudesData.ObtenerSolicitudes().Where(x => x.Folio == FolioSolicitud).FirstOrDefault();
                    rembolso.Solicitud = solicitud;
                    rembolso.Observacion = new Observacion();
                    rembolso.Observacion.Folio = Convert.ToInt16(Folio);
                    var comentarios = SolicitudesData.ObtenerComentario(Convert.ToInt32(Folio));
                    rembolso.comentarios = comentarios;
                }
                return View(rembolso);
            }
            catch (Exception e)
            {
                return RedirectToAction("AprobarSolicitud", "Aprobador");
            }

        }

        public ActionResult DetallesPorAutorizar(int Folio)
        {
            try
            {
                var SolicitudModel = new ObservacionViewModel();
                SolicitudModel.Solicitud = new Solicitud();
                SolicitudModel.Gasto = new Gasto();

                var Moneda = _MonedaData.ObtenerMonedas()
                    .OrderBy(x => x.Descripcion).ToList();
                SolicitudModel.Monedas = Moneda;
                var TipoSolicitud = SolicitudesData.ObtenerTipoSolicitud();
                var Solicitudes = SolicitudesData.SolicitudesXFolio(Folio);
                SolicitudModel.Solicitudes = TipoSolicitud;
                var Gastos = SolicitudesData.ObtenerGastos(Folio);
                var Destinos = SolicitudesData.DestinosXFolio(Folio);
                var comentarios = SolicitudesData.ObtenerComentario(Folio);
                SolicitudModel._GastosA = Gastos;
                SolicitudModel.Solicitud = Solicitudes;
                SolicitudModel.Destinos = Destinos;
                SolicitudModel.comentarios = comentarios;
                if (Solicitudes.Estatus == "Capturada" || Solicitudes.Estatus == "Incompleta" || Solicitudes.Estatus == "Rechazada")
                {
                    ViewBag.Deshabilitar = false;
                }
                else
                {
                    ViewBag.Deshabilitar = true;
                }
                return View(SolicitudModel);
            }
            catch (Exception)
            {

                return RedirectToAction("AprobarSolicitud", "Aprobador");
            }
            //return View(SolicitudModel);
        }

        public ActionResult DetallesComprobacion(string UUID)
        {
            var rembolso = new ObservacionViewModel();
            rembolso.Comprobante = _comprobante.ObtenerComprobantesXID(UUID);
            rembolso.Comprobante.ComprobanteXML = rembolso.Comprobante.Archivos.FirstOrDefault().Extension.Contains("xml");
            var gastos = _gastos.ObtenerGastos();
            rembolso.Gastos = gastos;
            return View(rembolso);
        }

        [HttpPost]
        public ActionResult Detalles(ObservacionViewModel viewModel)
        {
            int result = 0;
            if (viewModel.Operacion == 1)
            {
                string estatus = SolicitudesData.SolicitudesXFolio(viewModel.Observacion.Folio).Estatus;
                if (estatus.Equals("PorAutorizar"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "PorLiberar");
                }
                else if (estatus.Equals("Comprobada"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Revisada");
                }
            }
            else
            {
                result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Rechazada");
            }
            if (result!= 0)
            {
                var parametos = new Comentarios
                    { Comentario = viewModel.Observacion.Descripcion, Folio  = viewModel.Observacion.Folio };
                SolicitudesData.InsertarComentarios(parametos);
            }
            return RedirectToAction("AprobarSolicitud","Aprobador");
        }

    }
}
