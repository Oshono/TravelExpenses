using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;
using System.Web;
using Dapper;

namespace TravelExpenses.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudes _SolicitudesData;
        private readonly IDestinos _DestinosData;
        private readonly IUbicacion _UbicacionData;
        private readonly IMoneda _MonedaData;
        private readonly IEmpresa _EmpresaData;
        private readonly IGasto _GastoData;
        Random rd = new Random();
        public SolicitudesController(ISolicitudes SolicitudesData, IDestinos DestinosData,
                                        IUbicacion UbicacionData, IMoneda MonedaData, IEmpresa EmpresaData,
                                            IGasto GastoData)
        {
            this._SolicitudesData = SolicitudesData;
            this._DestinosData = DestinosData;
            this._UbicacionData = UbicacionData;
            this._MonedaData = MonedaData;
            this._EmpresaData = EmpresaData;
            this._GastoData = GastoData;

        }
        // GET: Solicitudes
        public ActionResult Index()
        {
            return View();

        }

        // GET: Solicitudes/Details/5
        public ActionResult ListarDestinos()
        {
            var destinos = _DestinosData.ObtenerDestinos(1);
            var empresaModel = new DestinosViewModel();
            empresaModel.Destinos = destinos;

            return View(empresaModel);
        }

        // GET: Solicitudes/Create
        public ActionResult Create()
        {
            List<Solicitud> Sol = new List<Solicitud>();
            Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
            HttpContext.Session.SetInt32("Folio", Convert.ToInt32(Sol[0].IdFolio) + 1);

            List<Solicitud> Solicitud = new List<Solicitud>();
            Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
            //ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

            ViewBag.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
            var SolicitudModel = new SolicitudesViewModel();

            var Pais = _UbicacionData.ObtenerPaises();
            var Estado = _UbicacionData.ObtenerEstados("");
            var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
            var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
            var Destino = _DestinosData.ObtenerDestinos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
            var Gasto = _SolicitudesData.ObtenerGastos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
            var Moneda = _MonedaData.ObtenerMonedas()
                          .OrderBy(x => x.Descripcion).ToList();
            var Empresa = _EmpresaData.ObtenerEmpresas();
            var _Gasto = _GastoData.ObtenerGastos();

            var IdFolio = _SolicitudesData.ObtenerIdSolicitud();
            SolicitudModel.Solicitudes = IdFolio;


            SolicitudModel.Solicitudes = TipoSolicitud;
            SolicitudModel.Paises = Pais;
            SolicitudModel.Estados = Estado;
            SolicitudModel.Ciudades = Ciudad;
            SolicitudModel.Destinos = Destino;
            SolicitudModel.Monedas = Moneda;
            SolicitudModel.Empresas = Empresa;
            SolicitudModel._Gastos = _Gasto;
            SolicitudModel.Gastos = Gasto;

            ViewBag.Hidden = true;
            return View(SolicitudModel);

        }
        public ActionResult ListarPaises()
        {
            var Pais = _UbicacionData.ObtenerPaises();
            var PaisesModel = new SolicitudesViewModel();
            PaisesModel.Paises = Pais;
            return View(PaisesModel);
        }
        [HttpPost]
        public ActionResult CargaCiudades(int IdEstado, string ClavePais)
        {
            var ciudades = _UbicacionData.ObtenerCiudades(ClavePais, IdEstado);
            return Json(ciudades);
        }

        [HttpPost]
        public ActionResult CargaEstados(string ClavePais)
        {
            var estados = _UbicacionData.ObtenerEstados(ClavePais);
            return Json(estados);
        }

        //public ActionResult Add()
        //{
        //    List<Solicitud> Solicitud = new List<Solicitud>();
        //    Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
        //    ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

        //    var SolicitudModel = new SolicitudesViewModel();
        //    SolicitudModel.Solicitudes = _SolicitudesData.ObtenerTipoSolicitud();
        //    SolicitudModel.Paises = _UbicacionData.ObtenerPaises();
        //    SolicitudModel.Estados = _UbicacionData.ObtenerEstados("");
        //    SolicitudModel.Ciudades = new List<Ciudades>();
        //    SolicitudModel.Destinos = _DestinosData.ObtenerDestinos(4);
        //    SolicitudModel.Monedas = _MonedaData.ObtenerMonedas();
        //    return View(SolicitudModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Add(SolicitudesViewModel Solicitud)
        //{
        ////    List<Solicitud> Sol = new List<Solicitud>();
        ////    Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
        ////    ViewBag.Sol = Convert.ToInt32(Sol[0].IdFolio) + 1;
        ////    Solicitud objsolicitudes = new Solicitud();

        ////    var objDestinos = new Destinos();
        ////    try
        ////    { 
        ////        //objsolicitudes.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
        ////        //objsolicitudes.IdTipoSolicitud = Solicitud.Solicitud.IdTipoSolicitud;
        ////        //objsolicitudes.Departamento = Solicitud.Solicitud.Departamento;
        ////        //objsolicitudes.Empresa = Solicitud.Solicitud.Empresa;
        ////        //objsolicitudes.ImporteSolicitado = Solicitud.Solicitud.ImporteSolicitado;
        ////        //objsolicitudes.ImporteComprobado = Solicitud.Solicitud.ImporteComprobado;
        ////        //objsolicitudes.Estatus = Solicitud.Solicitud.Estatus;
        ////        //objsolicitudes.IdEstado = Solicitud.Solicitud.IdEstado;
        ////        //objsolicitudes.Id = "1e882d25-59f5-4156-bd74-f33ae58a6e5a";
        ////        //objsolicitudes.RFC = "456777";
        ////        //objsolicitudes.ClaveMoneda = Solicitud.Moneda.ClaveMoneda;
        ////        ////_SolicitudesData.InsertarSolicitud(objsolicitudes);
        ////        //ViewBag.Script = "Datos almacenados";

        ////        //objDestinos.ClavePais = Solicitud.Pais.ClavePais;
        ////        //objDestinos.IdEstado = Solicitud.Estado.IdEstado;
        ////        //objDestinos.IdCiudad = Solicitud.Ciudad.IdCiudad;
        ////        //objDestinos.FechaSalida = Solicitud.Destino.FechaSalida;
        ////        //objDestinos.FechaLlegada = Solicitud.Destino.FechaLlegada;
        ////        //objDestinos.Folio = 13;
        ////        //objDestinos.Motivo = Solicitud.Destino.Motivo;

        ////        //_DestinosData.InsertarDestino(objDestinos);

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        ViewBag.Mensaje = "Error" + ex;
        ////    }
        //    return RedirectToAction("Create", "Solicitudes", new { ClavePais = Solicitud.Pais.ClavePais,Folio= Solicitud.Destino.Folio});
        //}

        // POST: Solicitudes/Create

        [HttpPost]
        public ActionResult Create(List<Solicitud> _solicitudes, List<Destinos> _Destino, List<Gasto> _Gasto)
        {
            _DestinosData.ObtenerDestino(1);
            var result = _DestinosData.ObtenerDestinos(4);
            Solicitud objsolicitudes = new Solicitud();


            Destinos destino = new Destinos();
            Gasto gasto = new Gasto();
            try
            {
                _solicitudes.ForEach(s =>
                {
                    objsolicitudes.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                    objsolicitudes.IdTipoSolicitud = s.IdTipoSolicitud;
                    objsolicitudes.Departamento = "TI";
                    objsolicitudes.Empresa = "";
                    objsolicitudes.ImporteSolicitado = s.ImporteSolicitado;
                    objsolicitudes.ImporteComprobado = s.ImporteComprobado;
                    objsolicitudes.Estatus = "SolicitudCapturada";
                    objsolicitudes.IdEstado = s.IdEstado;
                    objsolicitudes.Id = "1e882d25-59f5-4156-bd74-f33ae58a6e5a";
                    objsolicitudes.RFC = "456777";
                    objsolicitudes.ClaveMoneda = s.ClaveMoneda;
                    _SolicitudesData.InsertarSolicitud(objsolicitudes);
                });
                
                _Destino.ForEach(d =>
                {
                    destino.ClavePais = d.ClavePais;
                    destino.IdEstado = d.IdEstado;
                    destino.IdCiudad = d.IdCiudad;
                    destino.Motivo = d.Motivo;
                    destino.FechaSalida = d.FechaSalida;
                    destino.FechaLlegada = d.FechaLlegada;
                    destino.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                    _DestinosData.InsertarDestino(destino);
                });

                _Gasto.ForEach(g =>
                {
                    gasto.ClaveMoneda = g.ClaveMoneda;
                    gasto.MontoMaximo = "5000";
                    gasto.ImporteSolicitado = g.ImporteSolicitado;
                    gasto.TipoCambios = g.TipoCambios;
                    gasto.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                    gasto.RFC = "456777";
                    gasto.IdGasto = g.IdGasto;
                    _SolicitudesData.InsertarGastos(gasto);
                });

                return Json(gasto.Folio);

                #region Commented
                //    objDestinos.ClavePais = _solicitudes.Pais.ClavePais;
                //    objDestinos.IdEstado = _solicitudes.Estado.IdEstado;
                //    objDestinos.IdCiudad = _solicitudes.Ciudad.IdCiudad;
                //    objDestinos.FechaSalida = _solicitudes.Destino.FechaSalida;
                //    objDestinos.FechaLlegada = _solicitudes.Destino.FechaLlegada;
                //    objDestinos.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
                //    objDestinos.Motivo = _solicitudes.Destino.Motivo;
                //    //_DestinosData.InsertarDestino(objDestinos);

                //    objGastos.Folio = 12;//Convert.ToInt32(Sol[0].IdFolio) + 1;
                //    objGastos.MontoMaximo = "1000";
                //    objGastos.ImporteSolicitado = _solicitudes.Gasto.ImporteSolicitado;
                //    objGastos.TipoCambios = _solicitudes.Gasto.TipoCambios;
                //    objGastos.RFC = _solicitudes.Empresa.RFC;
                //    objGastos.IdGasto = _solicitudes._Gasto.IdGasto;
                //    objGastos.ClaveMoneda = _solicitudes.Gasto.ClaveMoneda;
                //    //_SolicitudesData.InsertarGastos(objGastos);

                //    ViewBag.Script = "Tienes que registrar al menos un destino y un concepto, la solicitud se registrará como incompleta";
                //}

                //    var Destino = _DestinosData.ObtenerDestinos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
                //var Gasto = _SolicitudesData.ObtenerGastos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));

                //var SolicitudModel = new SolicitudesViewModel();
                //var Pais = _UbicacionData.ObtenerPaises();
                //var Estado = _UbicacionData.ObtenerEstados("");
                //var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
                //var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                //var Empresa = _EmpresaData.ObtenerEmpresas();
                //var _Gasto = _GastoData.ObtenerGastos();

                //SolicitudModel.Solicitudes = TipoSolicitud;
                //SolicitudModel.Paises = Pais;
                //SolicitudModel.Estados = Estado;
                //SolicitudModel.Ciudades = Ciudad;
                //SolicitudModel.Monedas = _MonedaData.ObtenerMonedas();
                //SolicitudModel.Destinos = Destino;
                //SolicitudModel.Gastos = Gasto;
                //SolicitudModel.Empresas = Empresa;
                //SolicitudModel._Gastos = _Gasto;
                //ViewBag.Disabled = true;
                //ViewBag.Hidden = false;
                //return Json(_Destino);
                //return RedirectToAction("Create", "Solicitudes", new { Folio = Convert.ToInt32(TempData["Folio"]) });
                #endregion

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost]
        public ActionResult CreateIncomplete(SolicitudesViewModel _solicitudes)
        {
            //_DestinosData.ObtenerDestino(1);
            //var result = _DestinosData.ObtenerDestinos(4);
            var objsolicitudes = new Solicitud();
            var objDestinos = new Destinos();
            var objGastos = new Gasto();
            try
            {

                objsolicitudes.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                objsolicitudes.IdTipoSolicitud = _solicitudes.IdTipoSolicitud;
                objsolicitudes.Departamento = "TI";
                objsolicitudes.Empresa = "";
                objsolicitudes.ImporteSolicitado = _solicitudes.Solicitud.ImporteSolicitado;
                objsolicitudes.ImporteComprobado = _solicitudes.Solicitud.ImporteComprobado;
                objsolicitudes.Estatus = "Incompleta";
                objsolicitudes.IdEstado = _solicitudes.Solicitud.IdEstado;
                objsolicitudes.Id = "1e882d25-59f5-4156-bd74-f33ae58a6e5a";
                objsolicitudes.RFC = "456777";
                objsolicitudes.ClaveMoneda = _solicitudes.Moneda.ClaveMoneda;
                _SolicitudesData.InsertarSolicitud(objsolicitudes);


                return Json(objsolicitudes.Folio);



            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }


        [HttpPost]
        public ActionResult GuardarDestino(List<DestinoForm> Destino)
        {
            
            try
            {
                //var destino = new List<object>();
                //destino.Add(_DestinoModel.Pais.ClavePais);
                ////destino.Add(_DestinoModel.Estado.IdEstado.ToString());
                //destino.Add(_DestinoModel.Ciudad.IdCiudad.ToString());
                //destino.Add(_DestinoModel.Destino.FechaLlegada.ToString());
                //destino.Add(_DestinoModel.Destino.FechaSalida.ToString());
                //destino.Add(_DestinoModel.Destino.Motivo.ToString());

                //var lista = new[]{ destino };

                var lista = Destino;
                return Json(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult RegistrarDestino()
        {
            return View();
            //return Redirect("./ListarDestinos");
        }

        // GET: Solicitudes/Edit/5
        public ActionResult Edit(int Folio)
        {
            var SolicitudModel = new SolicitudesViewModel();
            var Destino = _DestinosData.ObtenerDestinos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
            SolicitudModel.Destinos = Destino;
            return View(SolicitudModel);

        }

        // POST: Solicitudes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            return View();
        }

        // GET: Solicitudes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solicitudes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}