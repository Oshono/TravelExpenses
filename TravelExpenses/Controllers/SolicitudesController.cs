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
using System.Security.Claims;

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
            try
            {

                var usuario=User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
                //HttpContext.Session.SetInt32("Folio", Convert.ToInt32(Sol[0].IdFolio) + 1);

                List<Solicitud> Solicitud = new List<Solicitud>();
                Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
                //ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

                //ViewBag.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                var SolicitudModel = new SolicitudesViewModel();

                var Pais = _UbicacionData.ObtenerPaises();
                var Estado = _UbicacionData.ObtenerEstados("");
                var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Destino = _DestinosData.ObtenerDestinos(0);
                //var Gasto = _SolicitudesData.ObtenerGastos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
                var Moneda = _MonedaData.ObtenerMonedas()
                              .OrderBy(x => x.Descripcion).ToList();
                var Empresa = _EmpresaData.ObtenerEmpresas();
                var _Gasto = _GastoData.ObtenerGastos();
                var politica = _SolicitudesData.ObtenerPoliticas(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
                //SolicitudModel.Gastos = Gasto;
                SolicitudModel.Politicas = politica;
                ViewBag.Hidden = true;

                return View(SolicitudModel);
            }
            catch (Exception)
            {
                var politica = _SolicitudesData.ObtenerPoliticas(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
                //HttpContext.Session.SetInt32("Folio", Convert.ToInt32(Sol[0].IdFolio));

                List<Solicitud> Solicitud = new List<Solicitud>();
                Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
                //ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

                //ViewBag.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                var SolicitudModel = new SolicitudesViewModel();

                var Pais = _UbicacionData.ObtenerPaises();
                var Estado = _UbicacionData.ObtenerEstados("");
                var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Destino = _DestinosData.ObtenerDestinos(0);
                //var Gasto = _SolicitudesData.ObtenerGastos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
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
                //SolicitudModel.Gastos = Gasto;
                SolicitudModel.Politicas = politica;
                ViewBag.Hidden = true;

                return View(SolicitudModel);
            }

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

        

        [HttpPost]
        public ActionResult Create(List<Solicitud> _solicitudes, List<Destinos> _Destino, List<Gasto> _Gasto, List<Comentarios> _comentarios)
        {
            _DestinosData.ObtenerDestino(1);
            var result = _DestinosData.ObtenerDestinos(4);
            Solicitud objsolicitudes = new Solicitud();
            var comentarios = new Comentarios();

            Destinos destino = new Destinos();
            Gasto gasto = new Gasto();
            try
            {
                _solicitudes.ForEach(s =>
                {
                    //objsolicitudes.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                    objsolicitudes.IdTipoSolicitud = s.IdTipoSolicitud;
                    objsolicitudes.Departamento = string.Empty;
                    objsolicitudes.Empresa = "";
                    objsolicitudes.ImporteSolicitado = s.ImporteSolicitado;
                    objsolicitudes.ImporteComprobado = s.ImporteComprobado;
                    objsolicitudes.Estatus = "Capturada";
                    objsolicitudes.IdEstado = s.IdEstado;
                    objsolicitudes.Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    objsolicitudes.RFC = string.Empty;
                    objsolicitudes.ClaveMoneda = string.Empty;
                    var Folio = _SolicitudesData.InsertarSolicitud(objsolicitudes);
                    HttpContext.Session.SetInt32("FolioSol",Folio );
                });

                _Destino.ForEach(d =>
                {
                    destino.ClavePais = d.ClavePais;
                    destino.IdEstado = d.IdEstado;
                    destino.IdCiudad = d.IdCiudad;
                    destino.Motivo = d.Motivo;
                    destino.FechaSalida = d.FechaSalida;
                    destino.FechaLlegada = d.FechaLlegada;
                    destino.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("FolioSol"));
                    _DestinosData.InsertarDestino(destino);
                });

                _Gasto.ForEach(g =>
                {
                    gasto.ClaveMoneda = g.ClaveMoneda;
                    gasto.MontoMaximo = 5000;
                    gasto.ImporteSolicitado = g.ImporteSolicitado;
                    gasto.TipoCambios = g.TipoCambios;
                    gasto.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("FolioSol"));
                    gasto.RFC = "456777";
                    gasto.IdGasto = g.IdGasto;
                    _SolicitudesData.InsertarGastos(gasto);
                });
                _comentarios.ForEach(c =>
                {
                    comentarios.Comentario = c.Comentario;
                    comentarios.Folio= Convert.ToInt32(HttpContext.Session.GetInt32("FolioSol"));
                    _SolicitudesData.InsertarComentarios(comentarios);
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
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CreateIncomplete(SolicitudesViewModel _solicitudes)
        { 
            var objsolicitudes = new Solicitud();
            var objDestinos = new Destinos();
            var objGastos = new Gasto();
            try
            {
                

                //objsolicitudes.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
                objsolicitudes.IdTipoSolicitud = _solicitudes.IdTipoSolicitud;
                objsolicitudes.Departamento = string.Empty; 
                objsolicitudes.Empresa = "";
                objsolicitudes.ImporteSolicitado = 0;
                objsolicitudes.ImporteComprobado = _solicitudes.Solicitud.ImporteComprobado;
                objsolicitudes.Estatus = "Incompleta";
                objsolicitudes.IdEstado = _solicitudes.Solicitud.IdEstado;
                objsolicitudes.Id =  User.FindFirst(ClaimTypes.NameIdentifier).Value;
                objsolicitudes.RFC = string.Empty;
                objsolicitudes.ClaveMoneda = string.Empty;
                var result= _SolicitudesData.InsertarSolicitud(objsolicitudes); 
                return Json(result);
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
            //    List<Solicitud> Sol = new List<Solicitud>();
            //    Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
            //HttpContext.Session.SetInt32("Folio", Convert.ToInt32(Sol[0].IdFolio) + 1);

            //List<Solicitud> Solicitud = new List<Solicitud>();
            //Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
            //ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

            //ViewBag.Folio = Convert.ToInt32(HttpContext.Session.GetInt32("Folio"));
            var SolicitudModel = new SolicitudesViewModel();

            var Pais = _UbicacionData.ObtenerPaises();
            var Estado = _UbicacionData.ObtenerEstados("");
            var Ciudad = _UbicacionData.ObtenerCiudades("", 0);
            var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud(); 
            var Moneda = _MonedaData.ObtenerMonedas()
                          .OrderBy(x => x.Descripcion).ToList();  

            var IdFolio = _SolicitudesData.ObtenerIdSolicitud();
            SolicitudModel.Solicitudes = IdFolio;
             
            SolicitudModel.Solicitudes = TipoSolicitud;
            SolicitudModel.Paises = Pais;
            SolicitudModel.Estados = Estado;
            SolicitudModel.Ciudades = Ciudad; 
            SolicitudModel.Monedas = Moneda;  
            
            return View(SolicitudModel);
        }

        [HttpPost]
        public ActionResult RegistrarDestino(List<Destinos> _destinos)
        {
            var SolicitudModel = new SolicitudesViewModel();
            var objDestinos = new Destinos();
            Destinos obj = new Destinos();


            var Pais = _UbicacionData.ObtenerPaises();
            var Estado = _UbicacionData.ObtenerEstados("");

            var Ciudad = _UbicacionData.ObtenerCiudades("", 0);
            try
            {
                _destinos.ForEach(d =>
                {
                     
                    objDestinos.FechaSalida = d.FechaSalida;
                    objDestinos.FechaLlegada = d.FechaLlegada;
                    objDestinos.ClavePais = d.ClavePais;
                    objDestinos.IdCiudad = d.IdCiudad;
                    objDestinos.IdEstado = d.IdEstado;
                    objDestinos.Motivo = d.Motivo;
                    objDestinos.IdDestinos = d.IdDestinos;
                    objDestinos.Folio = d.Folio;

                    _DestinosData.InsertarDestino(objDestinos);
                });
                return Json(objDestinos);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // GET: Solicitudes/Edit/5
        public ActionResult Edit(int Folio)
        {
            //var Gasto = _SolicitudesData.ObtenerGastos(Convert.ToInt32(HttpContext.Session.GetInt32("Folio")));
            try
            {


                var SolicitudModel = new SolicitudesViewModel();
                SolicitudModel.Solicitud = new Solicitud();
                SolicitudModel.Gasto = new Gasto();

                var Moneda = _MonedaData.ObtenerMonedas()
                              .OrderBy(x => x.Descripcion).ToList();
                SolicitudModel.Monedas = Moneda;
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Solicitudes = _SolicitudesData.SolicitudesXFolio(Folio);
                SolicitudModel.Solicitudes = TipoSolicitud;
                var Gastos = _SolicitudesData.ObtenerGastos(Folio);
                var Destinos = _SolicitudesData.DestinosXFolio(Folio);
                var comentarios = _SolicitudesData.ObtenerComentario(Folio);
                SolicitudModel.Gastos = Gastos;
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

                return Redirect("../");
            }

        }

        
        // POST: Solicitudes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            return View();
        }


        public ActionResult EditarDestino(int IdDestinos)
        {
            var SolicitudModel = new SolicitudesViewModel();
            //SolicitudModel.Solicitud = new Solicitud();
            //var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud(); 
            //SolicitudModel.Solicitudes = TipoSolicitud;

            SolicitudModel.Pais= new Paises();

            var destinos = _SolicitudesData.ObtenerDestinosFolio(IdDestinos);

            var Pais = _UbicacionData.ObtenerPaises(); 
            SolicitudModel.Paises = Pais;
            var Estado = _UbicacionData.ObtenerEstados(destinos.ClavePais);

            var Ciudad = _UbicacionData.ObtenerCiudades(destinos.ClavePais,destinos.IdEstado);

            SolicitudModel.Estados = Estado;
            SolicitudModel.Destino = destinos; 
            SolicitudModel.Ciudades = Ciudad;

             
            return View(SolicitudModel); 
            
        }

        [HttpPost]
        public ActionResult EditarDestino(SolicitudesViewModel _destinos)
        {
            var SolicitudModel = new SolicitudesViewModel();
            var objDestinos = new Destinos();
            Destinos obj = new Destinos();


            var Pais = _UbicacionData.ObtenerPaises();
            var Estado = _UbicacionData.ObtenerEstados("");

            var Ciudad = _UbicacionData.ObtenerCiudades("", 0);
            try
            {
                SolicitudModel.Estados = Estado;
                SolicitudModel.Ciudades = Ciudad;
                SolicitudModel.Paises = Pais;

                objDestinos.FechaSalida = _destinos.Destino.FechaSalida;
                objDestinos.FechaLlegada = _destinos.Destino.FechaLlegada;
                objDestinos.ClavePais = _destinos.Destino.ClavePais;
                objDestinos.IdCiudad = _destinos.Destino.IdCiudad;
                objDestinos.IdEstado = _destinos.Destino.IdEstado;
                objDestinos.Motivo = _destinos.Destino.Motivo;
                objDestinos.IdDestinos = _destinos.Destino.IdDestinos;
                _SolicitudesData.Destinos_Upd(objDestinos);
                return Json(objDestinos);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult EditarGastos(int IdGastos)
        {
            var SolicitudModel = new SolicitudesViewModel();
            var Gastos_ = _SolicitudesData.ObtenerGastosFolio(IdGastos);
            var _Gasto = _GastoData.ObtenerGastos();
            var Moneda = _MonedaData.ObtenerMonedas()
                          .OrderBy(x => x.Descripcion).ToList();

            SolicitudModel.Gasto = Gastos_;
            SolicitudModel._Gastos = _Gasto; 
            SolicitudModel.Monedas = Moneda;
            return View(SolicitudModel);
            
        }

        [HttpPost]
        public ActionResult EditarGastos(SolicitudesViewModel Gastos_)
        {
            try
            {
                var objDestinos = new Gasto();
                objDestinos.ImporteSolicitado = Gastos_.Gasto.ImporteSolicitado;
                objDestinos.TipoCambios = Gastos_.Gasto.TipoCambios;
                objDestinos.IdGasto = Gastos_.Gasto.IdGasto;
                objDestinos.ClaveMoneda = Gastos_.Gasto.ClaveMoneda;
                objDestinos.IdGastos = Gastos_.Gasto.IdGastos;
                _SolicitudesData.Gastos_Upd(objDestinos);
                return Json(objDestinos);
            }
            catch (Exception ex)
            {

                throw ex;
            } 

        }

        [HttpPost]
        public ActionResult EliminarDestino(int IdDestinos)
        {
            try
            {
                _SolicitudesData.EliminarDestinos(IdDestinos);
                return Json(IdDestinos);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult EliminarGasto(int IdGastos)
        {
            try
            {
                _SolicitudesData.EliminarGastos(IdGastos);
                return Json(IdGastos);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult ModificarSolicitud(SolicitudesViewModel _Solicitud)
        {
            var objSolicitud = new Solicitud();
            try
            {

                objSolicitud.Folio = _Solicitud.Solicitud.Folio;
                objSolicitud.ClaveMoneda = "MXN";
                objSolicitud.IdTipoSolicitud = _Solicitud.Solicitud.IdTipoSolicitud;
                objSolicitud.ImporteSolicitado = _Solicitud.Solicitud.ImporteSolicitado;
                _SolicitudesData.SolicitudesUpd(objSolicitud);
                return Json(objSolicitud);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public ActionResult RegistrarGasto()
        {
            
            var SolicitudModel = new SolicitudesViewModel();
             
            var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
            var _Gasto = _GastoData.ObtenerGastos();
            var Moneda = _MonedaData.ObtenerMonedas()
                          .OrderBy(x => x.Descripcion).ToList();
            SolicitudModel._Gastos = _Gasto;
            SolicitudModel.Solicitudes = TipoSolicitud;

            SolicitudModel.Monedas = Moneda;
            return View(SolicitudModel);
        }

        public ActionResult Comentarios()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Comentarios(List<Comentarios> _comentarios)
        {
            var comentarios = new Comentarios();
            try
            {
                _comentarios.ForEach(c=>{
                    comentarios.Comentario = c.Comentario;
                    comentarios.Folio = c.Folio;
                    _SolicitudesData.InsertarComentarios(comentarios);
                });
                
                return Json(1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult RegistrarGasto(List<Gasto>_gastos)
        {
            var gastos = new Gasto();
            try
            {


                _gastos.ForEach(g =>
                {
                    gastos.ClaveMoneda = g.ClaveMoneda;
                    gastos.IdGasto = g.IdGasto;
                    gastos.ImporteSolicitado = g.ImporteSolicitado;
                    gastos.TipoCambios = g.TipoCambios;
                    gastos.Folio = g.Folio;
                    gastos.MontoMaximo = 5000;
                    gastos.RFC = "456777";
                    _SolicitudesData.InsertarGastos(gastos);

                });
                return Json(gastos);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}