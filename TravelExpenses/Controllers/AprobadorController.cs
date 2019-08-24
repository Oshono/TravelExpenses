using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.Services;
using TravelExpenses.TravelExpenses.Data;
using TravelExpenses.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelExpenses.Controllers
{
    [Authorize]
    public class AprobadorController : Controller
    {
        private readonly ISolicitudes SolicitudesData;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IObservacionDA _ObservacionDA;
        private readonly IMoneda _MonedaData;
        private readonly IEmailSender Email;
        private readonly ICentroCosto _centroCosto;
        private readonly IUsuario Usuario;
        public AprobadorController(ISolicitudes solicitudes,
            IComprobante comprobante,
            IGasto gastos,
            IObservacionDA ObservacionDA,
            IMoneda MonedaData,
            IHostingEnvironment env,
            IEmailSender email,
            ICentroCosto centroCosto,
            IUsuario usuario)
        {
            SolicitudesData = solicitudes;
            _comprobante = comprobante;
            _gastos = gastos;
            _ObservacionDA = ObservacionDA;
            this._MonedaData = MonedaData;
            _env = env;
            Email = email;
            _centroCosto = centroCosto;
            Usuario = usuario;
        }


        // GET: /<controller>/

        public ActionResult AprobarSolicitud()
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();

            var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("Comprobada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorAutorizar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("Comprobada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorComprobar;
                return Json(PorComprobar);
            }
            else if (estatus.Equals("PorAutorizar"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorAutorizar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("Comprobada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorAutorizar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return Json(solicitud.Solicitudes);
            }

        }



        public ActionResult Aprobar(string Folio)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            solicitud.Solicitudes = SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return View("AprobarSolicitud", solicitud);
        }
        public ActionResult Detalles(string Folio)
        {
            var rembolso = new ObservacionViewModel();
            int FolioSolicitud = 0;
            try
            {
                if (int.TryParse(Folio, out FolioSolicitud))
                {
                    rembolso.Comprobantes = new List<Comprobante>();
                    rembolso.Comprobantes = _comprobante.ObtenerComprobantes(FolioSolicitud);

                    foreach (Comprobante comp in rembolso.Comprobantes)
                    {
                        if (comp.FormaPago == "01" && comp.Conceptos.Where(x => x.DescripcionProdServ.ToUpper().Contains("GASOLINA") || x.DescripcionProdServ.ToUpper().Contains("ALIMENTO")).Count() > 0)
                        {
                            comp.MensajeError = "La forma de pago para Gasolina o Alimenos debe ser diferente de Efectivo";
                        }
                    }

                    var solicitud = SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value).Where(x => x.Folio == FolioSolicitud).FirstOrDefault();
                    rembolso.solicitud = solicitud;
                    rembolso.DetallesAsociados = rembolso.Comprobantes.Where(x => x.Conceptos.Count(y => y.IdGasto == 0) > 0).Count() < 1;
                }
                var _Gasto = _gastos.ObtenerGastos();

                var misGastos = SolicitudesData.ObtenerGastos(FolioSolicitud);
                rembolso.MisGastos = misGastos;
                var comentarios = SolicitudesData.ObtenerComentario(Convert.ToInt32(Folio));
                rembolso.comentarios = comentarios;
                rembolso.Observacion = new Observacion();
                rembolso.Observacion.Folio = Convert.ToInt32(Folio);
                return View(rembolso);
            }
            catch (Exception e)
            {
                return RedirectToAction("AprobarSolicitud", "Aprobador");
            }


            //try
            //{
            //    int FolioSolicitud = 0;

            //    if (int.TryParse(Folio, out FolioSolicitud))
            //    {
            //        rembolso.Comprobantes = new List<Comprobante>();
            //        rembolso.Comprobantes = _comprobante.ObtenerComprobantes(FolioSolicitud);
            //        var solicitud = SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value).Where(x => x.Folio == FolioSolicitud).FirstOrDefault();
            //        rembolso.Solicitud = solicitud;
            //        rembolso.Observacion = new Observacion();
            //        rembolso.Observacion.Folio = Convert.ToInt16(Folio);
            //        var comentarios = SolicitudesData.ObtenerComentario(Convert.ToInt32(Folio));
            //        rembolso.comentarios = comentarios;
            //        rembolso.Observacion = new Observacion();
            //        rembolso.Observacion.Folio = Convert.ToInt32(Folio);
            //    }
            //    return View(rembolso);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("AprobarSolicitud", "Aprobador");
            //}

        }
        private readonly IHostingEnvironment _env;

        string[] contentType = new string []{".aac|audio/aac"
,".abw|application/x-abiword"
,".arc|application/octet-stream"
,".avi|video/x-msvideo"
,".azw|application/vnd.amazon.ebook"
,".bin|application/octet-stream"
,".bz|application/x-bzip"
,".bz2|application/x-bzip2"
,".csh|application/x-csh"
,".css|text/css"
,".csv|text/csv"
,".doc|application/msword"
,".epub|application/epub+zip"
,".gif|image/gif"
,".htm|text/html"
,".html|text/html"
,".ico|image/x-icon"
,".ics|text/calendar"
,".jar|application/java-archive"
,".jpeg|image/jpeg"
,".jpg|image/jpeg"
,".js|application/javascript"
,".json|application/json"
,".mpeg|Video MPEG	video/mpeg"
,".odp|application/vnd.oasis.opendocument.presentation"
,".ods|application/vnd.oasis.opendocument.spreadsheet"
,".odt|application/vnd.oasis.opendocument.text"
,".oga|audio/ogg"
,".ogv|video/ogg"
,".ogx|application/ogg"
,".pdf|application/pdf"
,".ppt|application/vnd.ms-powerpoint"
,".rar|application/x-rar-compressed"
,".rtf|application/rtf"
,".sh|application/x-sh"
,".svg|image/svg+xml"
,".swf|application/x-shockwave-flash"
,".tar|application/x-tar"
,".tiff|image/tiff"
,".ttf|font/ttf"
,".vsd|application/vnd.visio"
,".wav|audio/x-wav"
,".weba|audio/webm"
,".webm|video/webm"
,".webp|image/webp"
,".woff|font/woff"
,".woff2|font/woff2"
,".xhtml|application/xhtml+xml"
,".xls|application/vnd.ms-excel"
,".xml|application/xml"
,".xul|application/vnd.mozilla.xul+xml"};

        public FileResult GetReport(string Ruta)
        {
            string[] rutas = Ruta.Split('/');
            var path = Path.Combine(_env.ContentRootPath, rutas[1]+"s", rutas[2]);
            byte[] FileBytes = System.IO.File.ReadAllBytes(path);
            string contentTypemini = "";
            string extencion = Path.GetExtension(Ruta);
            for (int i = 0; i < contentType.Length; i++)
            {
                string[] ca = contentType[i].Split('|');
                if (ca[1].ToLower().Equals(extencion.ToLower()))
                {
                    contentTypemini = ca[0];
                    break;
                }
            }
            return File(FileBytes, contentTypemini);
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
            string estatu = "";
            if (viewModel.Operacion == 1)
            {
                var estatus = SolicitudesData.SolicitudesXFolio(viewModel.Observacion.Folio).Estatus;

                if (estatus.Equals("PorAutorizar"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "PorLiberar");
                    estatu = "PorLiberar";
                    ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Autorización Solicitud" + viewModel.Observacion.Folio.ToString(),
                        "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, PorLiberar");

                }
                else if (estatus.Equals("Comprobada"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Revisada");
                    estatu = "Revisada";
                    ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Autorización Solicitud" + viewModel.Observacion.Folio.ToString(),
                        "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, Revisada");
                }
            }
            else
            {
                result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Rechazada");
                ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Rechazada Solicitud" + viewModel.Observacion.Folio.ToString(),
                    "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, Rechazada");
                estatu = "Rechazada";
            }

            if (result!= 0)
            {
                var parametos = new Comentarios
                    { Comentario = viewModel.Observacion.Descripcion, Folio  = viewModel.Observacion.Folio,estatus = estatu};
                SolicitudesData.InsertarComentarios(parametos);
            }
            return RedirectToAction("AprobarSolicitud","Aprobador");
        }


        public bool ObtenerCorreos(int Folio,
            string subject,
            string message
        )
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var centroCosto = _centroCosto.ConsultarControCostoPorUsuario(id).FirstOrDefault().ClaveCentroCosto;

            var Correos = Usuario.ObtenerCorreos(username, centroCosto, Folio);
            SendEmail(Correos.Solicitante, subject, message, Correos.Aprobador);
            SendEmail(Correos.Solicitante, subject, message, Correos.Procesador);

            return true;
        }
        public Task SendEmail(string email, string subject, string message, string CC)
        {
            return Email.SendEmailCCAsync(email, CC, subject, message);
        }
    }
}
