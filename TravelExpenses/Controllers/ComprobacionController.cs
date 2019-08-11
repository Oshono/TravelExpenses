using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{

    public class ComprobacionController : Controller
    {
        private readonly ISolicitudes SolicitudesData;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IObservacionDA _ObservacionDA;
        private readonly IMoneda _MonedaData;
        public ComprobacionController(ISolicitudes solicitudes,
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

        public ActionResult ComprobacionSolicitud()
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("Revisada");
            var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("PorLiberar");
            solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
            var Estatus =
                    (from e in SolicitudesData.EstatusSolicitudes()
                     where e.Status == "PorLiberar" | e.Status == "Revisada"
                     select e
                        );
            solicitud.Estatuses = Estatus;
            return View(solicitud);
        }

        public ActionResult SolicitudCerradas()
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("Cerrada");
            solicitud.Solicitudes = PorComprobar;
            var Estatus =
                (from e in SolicitudesData.EstatusSolicitudes()
                    where e.Status == "Cerrada"
                    select e
                );
            solicitud.Estatuses = Estatus;
            return View(solicitud);
        }

        public IActionResult SolicitudesEstatus(string estatus)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            if (estatus.Equals("PorLiberar"))
            {
                var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("PorLiberar");
                solicitud.Solicitudes = PorComprobar;
                return Json(PorComprobar);
            }
            else if (estatus.Equals("Revisada"))
            {
                var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("Revisada");
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else
            {
                var PorComprobar = SolicitudesData.ObtenerSolicitudesXEstatus("PorLiberar");
                var PorAutorizar = SolicitudesData.ObtenerSolicitudesXEstatus("Revisada");
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return Json(solicitud.Solicitudes);
            }

        }



        public ActionResult Detalles(string Folio)
        {
            try
            {
                int FolioSolicitud = 0;
                var rembolso = new ObservacionViewModel();
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
                    rembolso.Observacion = new Observacion();
                    rembolso.Observacion.Folio = Convert.ToInt32(Folio);
                }
                return View(rembolso);
            }
            catch (Exception e)
            {
                return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
            }
            
        }

        public ActionResult DetallesSolicitudCerradas(string Folio)
        {
            try
            {
                int FolioSolicitud = 0;
                var rembolso = new ObservacionViewModel();
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
                    rembolso.Observacion = new Observacion();
                    rembolso.Observacion.Folio = Convert.ToInt32(Folio);
                }
                return View(rembolso);
            }
            catch (Exception e)
            {
                return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
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
                SolicitudModel.Observacion = new Observacion();
                SolicitudModel.Observacion.Folio = Folio;
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

                return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
            }

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
                if (estatus.Equals("PorLiberar"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "PorComprobar");
                }
                else if (estatus.Equals("Revisada"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Cerrada");
                }
            }
            else
            {
                result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Rechazada");
            }
            if (result != 0)
            {
                var parametos = new Comentarios
                    { Comentario = viewModel.Observacion.Descripcion, Folio = viewModel.Observacion.Folio };
                SolicitudesData.InsertarComentarios(parametos);
            }
            return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
        }

    }

}
