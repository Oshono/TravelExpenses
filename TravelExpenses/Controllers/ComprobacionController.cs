using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.Services;
using TravelExpenses.TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{
    [Authorize]
    public class ComprobacionController : Controller
    {
        private readonly ISolicitudes SolicitudesData;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IObservacionDA _ObservacionDA;
        private readonly IMoneda _MonedaData;
        private readonly IEmailSender Email;
        private readonly ICentroCosto _centroCosto;
        private readonly IUsuario Usuario;
        public ComprobacionController(ISolicitudes solicitudes,
                    IComprobante comprobante,
                    IGasto gastos,
                    IObservacionDA ObservacionDA,
                    IMoneda MonedaData
                    , IHostingEnvironment hostingEnvironment,
                    IEmailSender email,
                    ICentroCosto centroCosto,
                    IUsuario usuario)
        {
            SolicitudesData = solicitudes;
            _comprobante = comprobante;
            _gastos = gastos;
            _ObservacionDA = ObservacionDA;
            this._MonedaData = MonedaData;
            _hostingEnvironment = hostingEnvironment;
            Email = email;
            _centroCosto = centroCosto;
            Usuario = usuario;
        }


        // GET: /<controller>/

        public ActionResult ComprobacionSolicitud()
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
            var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada????", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            solicitud.Solicitudes = PorComprobar;
            var Estatus =
                (from e in SolicitudesData.EstatusSolicitudes()
                    where e.Status == "Cerrada" | e.Status == "PorLiberar" | e.Status == "Revisada" | e.Status== "PorComprobar" | e.Status == "Liberada"
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
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorComprobar;
                return Json(PorComprobar);
            }
            else if (estatus.Equals("Revisada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("PorComprobar"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorComprobar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("Liberada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Liberada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("Todo"))
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return Json(solicitud.Solicitudes);
            }
            else if (estatus.Equals("Cerrada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else
            {
                var PorLiberar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = PorLiberar.Union(PorAutorizar);
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorComprobar", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var Liberada = _ObservacionDA.ObtenerSolicitudesXEstatus("Liberada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result2 = PorComprobar.Union(Liberada);
                var Cerrada = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                solicitud.Solicitudes = result.Union(result2).Union(Cerrada);
                return Json(solicitud.Solicitudes);
            }
        }

        public IActionResult SolicitudesEstatusFechas(string estatus,DateTime inicio,DateTime fin)
        {
            ObservacionViewModel solicitud = new ObservacionViewModel();
            string ID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (estatus.Equals("PorLiberar"))
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", ID, inicio,fin);
                solicitud.Solicitudes = PorComprobar;
                return Json(PorComprobar);
            }
            else if (estatus.Equals("Revisada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("PorComprobar"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorComprobar", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("Liberada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Liberada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);

            }
            else if (estatus.Equals("Todo"))
            {
                var PorComprobar = _ObservacionDA.ObtenerSolicitudesXEstatus("PorLiberar", ID, inicio, fin);
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Revisada", ID, inicio, fin);
                solicitud.Solicitudes = PorComprobar.Union(PorAutorizar);
                return Json(solicitud.Solicitudes);
            }
            else if (estatus.Equals("Cerrada"))
            {
                var PorAutorizar = _ObservacionDA.ObtenerSolicitudesXEstatus("Cerrada", ID, inicio, fin);
                solicitud.Solicitudes = PorAutorizar;
                return Json(PorAutorizar);
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
                return Json(solicitud.Solicitudes);
            }

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
            //    var rembolso = new ObservacionViewModel();
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
            //    return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
            //}
            
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
                    var solicitud = SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value).Where(x => x.Folio == FolioSolicitud).FirstOrDefault();
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
            string estatu = "";
            if (viewModel.Operacion == 1)
            {
                string estatus = SolicitudesData.SolicitudesXFolio(viewModel.Observacion.Folio).Estatus;
                if (estatus.Equals("PorLiberar"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "PorComprobar");
                    estatu = "PorComprobar";
                    ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Autorización Solicitud" + viewModel.Observacion.Folio.ToString(),
                        "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, PorComprobar");

                }
                else if (estatus.Equals("Revisada"))
                {
                    result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Cerrada");
                    estatu = "Cerrada";
                    ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Autorización Solicitud" + viewModel.Observacion.Folio.ToString(),
                        "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, Cerrada");
                }
            }
            else
            {
                result = SolicitudesData.ActualizarEstatus(viewModel.Observacion.Folio, "Rechazada");
                ObtenerCorreos(viewModel.Observacion.Folio, "Solicitud Rechazada Solicitud" + viewModel.Observacion.Folio.ToString(),
                    "Favor de actualizar la solicitud " + viewModel.Observacion.Folio.ToString() + " Estatus Actual, Rechazada");
                estatu = "Rechazada";
            }
            if (result != 0)
            {
                var parametos = new Comentarios
                    { Comentario = viewModel.Observacion.Descripcion, Folio = viewModel.Observacion.Folio,estatus = estatu};
                SolicitudesData.InsertarComentarios(parametos);
            }
            return RedirectToAction("ComprobacionSolicitud", "Comprobacion");
        }

        private IHostingEnvironment _hostingEnvironment;

        [HttpPost]
        public async Task<IActionResult> OnPostExport(ObservacionViewModel viewModel)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"solicitudes.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Solicitud");
               
                ICellStyle style = workbook.CreateCellStyle();
                style.FillBackgroundColor = IndexedColors.Aqua.Index;
                style.FillPattern = FillPattern.BigSpots;

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Folio Solicitud");
                row.CreateCell(1).SetCellValue("Tipo Solicitud");
                row.CreateCell(2).SetCellValue("Nombre Solicitante");
                row.CreateCell(3).SetCellValue("Importe Solicitado");
                row.CreateCell(4).SetCellValue("Fecha Autorización");
                int contador = 1;
                string array = viewModel.Array;
                var parameto = JsonConvert.DeserializeObject<List<Solicitud>>(array);
                parameto.ForEach(x =>
                {
                    if (x.Exportar)
                    {
                        _ObservacionDA.SolicitudesExportadas_ins(x.Folio);
                        row = excelSheet.CreateRow(contador);
                        row.CreateCell(0).SetCellValue(x.Folio);
                        row.CreateCell(1).SetCellValue(x.Descripcion);
                        row.CreateCell(2).SetCellValue(x.UserName);
                        row.CreateCell(3).SetCellValue(x.ImporteSolicitado);
                        row.CreateCell(4).SetCellValue(x.FechaSolicitud.ToString("dd/MM/yyyy"));
                        contador++;
                    }
                });
                
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public IActionResult SelecionarSolicitudes(string parameto,int Folio,bool valor)
        {
            var parame = JsonConvert.DeserializeObject<List<Solicitud>>(parameto);
            parame.ForEach(x =>
            {
                if (!x.ExportarRealizada)
                {
                    if (Folio == x.Folio)
                    {
                        x.Exportar = valor;
                    }
                }
            });
            return Json(parame);
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
