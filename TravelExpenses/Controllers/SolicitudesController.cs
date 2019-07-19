using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudes _SolicitudesData;
        private readonly IDestinos _DestinosData;
        private readonly IUbicacion _UbicacionData;
        private readonly IMoneda _MonedaData;
        Random rd = new Random();
        public SolicitudesController(ISolicitudes SolicitudesData, IDestinos DestinosData, IUbicacion UbicacionData, IMoneda MonedaData)
        {
            this._SolicitudesData = SolicitudesData;
            this._DestinosData = DestinosData;
            this._UbicacionData = UbicacionData;
            this._MonedaData = MonedaData;


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
            List<Solicitud> Solicitud = new List<Solicitud>();
            Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
            ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

            var SolicitudModel = new SolicitudesViewModel();

            var Pais = _UbicacionData.ObtenerPaises();
            var Estado = _UbicacionData.ObtenerEstados("MEX");
            var Ciudad = _UbicacionData.ObtenerCiudades("MEX",1);
            var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
            var Destino = _DestinosData.ObtenerDestinos(Convert.ToInt32(Solicitud[0].IdFolio) + 1);
            var Moneda = _MonedaData.ObtenerMonedas();


            var IdFolio = _SolicitudesData.ObtenerIdSolicitud();
            SolicitudModel.Solicitudes = IdFolio;
            
            
            SolicitudModel.Solicitudes = TipoSolicitud;
            SolicitudModel.Paises = Pais;
            SolicitudModel.Estados = Estado;
            SolicitudModel.Ciudades = Ciudad;
            SolicitudModel.Destinos = Destino;
            SolicitudModel.Monedas = Moneda;
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

        public ActionResult Add()
        {
            List<Solicitud> Solicitud = new List<Solicitud>();
            Solicitud = _SolicitudesData.ObtenerIdSolicitud().ToList();
            ViewBag.Sol = Convert.ToInt32(Solicitud[0].IdFolio) + 1;

            var SolicitudModel = new SolicitudesViewModel();
            SolicitudModel.Solicitudes = _SolicitudesData.ObtenerTipoSolicitud();
            SolicitudModel.Paises = _UbicacionData.ObtenerPaises();
            SolicitudModel.Estados = _UbicacionData.ObtenerEstados("");
            SolicitudModel.Ciudades = new List<Ciudades>();
            SolicitudModel.Destinos = _DestinosData.ObtenerDestinos(4);
            SolicitudModel.Monedas = _MonedaData.ObtenerMonedas();
            return View(SolicitudModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SolicitudesViewModel Solicitud)
        {
            List<Solicitud> Sol = new List<Solicitud>();
            Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
            ViewBag.Sol = Convert.ToInt32(Sol[0].IdFolio) + 1;
            Solicitud objsolicitudes = new Solicitud();

            var objDestinos = new Destinos();
            try
            { 
                objsolicitudes.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
                objsolicitudes.IdTipoSolicitud = Solicitud.Solicitud.IdTipoSolicitud;
                objsolicitudes.Departamento = Solicitud.Solicitud.Departamento;
                objsolicitudes.Empresa = Solicitud.Solicitud.Empresa;
                objsolicitudes.ImporteSolicitado = Solicitud.Solicitud.ImporteSolicitado;
                objsolicitudes.ImporteComprobado = Solicitud.Solicitud.ImporteComprobado;
                objsolicitudes.Estatus = Solicitud.Solicitud.Estatus;
                objsolicitudes.IdEstado = Solicitud.Solicitud.IdEstado;
                objsolicitudes.Id = "1e882d25-59f5-4156-bd74-f33ae58a6e5a";
                objsolicitudes.RFC = "456777";
                objsolicitudes.ClaveMoneda = Solicitud.Moneda.ClaveMoneda;
                //_SolicitudesData.InsertarSolicitud(objsolicitudes);
                ViewBag.Script = "Datos almacenados";

                objDestinos.ClavePais = Solicitud.Pais.ClavePais;
                objDestinos.IdEstado = Solicitud.Estado.IdEstado;
                objDestinos.IdCiudad = Solicitud.Ciudad.IdCiudad;
                objDestinos.FechaSalida = Solicitud.Destino.FechaSalida;
                objDestinos.FechaLlegada = Solicitud.Destino.FechaLlegada;
                objDestinos.Folio = 13;
                objDestinos.Motivo = Solicitud.Destino.Motivo;

                //_DestinosData.InsertarDestino(objDestinos);

            }
            catch (Exception ex)
            {

                ViewBag.Mensaje = "Error" + ex;
            }
            return RedirectToAction("Create", "Solicitudes", new { ClavePais = Solicitud.Pais.ClavePais,Folio= Solicitud.Destino.Folio});
        }

        // POST: Solicitudes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudesViewModel _solicitudes)
        {

            List<Solicitud> Sol = new List<Solicitud>();
            Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
            ViewBag.Sol = Convert.ToInt32(Sol[0].IdFolio) + 1;


            _DestinosData.ObtenerDestino(1);
            var result = _DestinosData.ObtenerDestinos(4);
            Solicitud objsolicitudes = new Solicitud();
            var objDestinos = new Destinos();

            try
            {

                var SolicitudModel = new SolicitudesViewModel();
                var Pais = _UbicacionData.ObtenerPaises();
                var Estado = _UbicacionData.ObtenerEstados("MEX");
                var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
              


                SolicitudModel.Solicitudes = TipoSolicitud;
                SolicitudModel.Paises = Pais;
                SolicitudModel.Estados = Estado;
                SolicitudModel.Ciudades = Ciudad;
                SolicitudModel.Monedas = _MonedaData.ObtenerMonedas();

                if (_solicitudes.Pais.ClavePais == null) {

                    ViewBag.Script = "Debes registrar al menos un destino";
                }
                else
                {

                    objsolicitudes.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
                    objsolicitudes.IdTipoSolicitud = _solicitudes.Solicitud.IdTipoSolicitud;
                    objsolicitudes.Departamento = "TI";
                    objsolicitudes.Empresa = "";
                    objsolicitudes.ImporteSolicitado = _solicitudes.Solicitud.ImporteSolicitado;
                    objsolicitudes.ImporteComprobado = _solicitudes.Solicitud.ImporteComprobado;
                    objsolicitudes.Estatus = _solicitudes.Solicitud.Estatus;
                    objsolicitudes.IdEstado = _solicitudes.Solicitud.IdEstado;
                    objsolicitudes.Id = "1e882d25-59f5-4156-bd74-f33ae58a6e5a";
                    objsolicitudes.RFC = "456777";
                    objsolicitudes.ClaveMoneda = _solicitudes.Moneda.ClaveMoneda;
                    _SolicitudesData.InsertarSolicitud(objsolicitudes);

                    objDestinos.ClavePais = _solicitudes.Pais.ClavePais;
                    objDestinos.IdEstado = _solicitudes.Estado.IdEstado;
                    objDestinos.IdCiudad = _solicitudes.Ciudad.IdCiudad;
                    objDestinos.FechaSalida = _solicitudes.Destino.FechaSalida;
                    objDestinos.FechaLlegada = _solicitudes.Destino.FechaLlegada;
                    objDestinos.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
                    objDestinos.Motivo = _solicitudes.Destino.Motivo;
                    _DestinosData.InsertarDestino(objDestinos);

                    ViewBag.Script = "Datos almacenados";
                }

                var Destino = _DestinosData.ObtenerDestinos(objDestinos.Folio);

                SolicitudModel.Destinos = Destino;
                
                return View(SolicitudModel);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult RegistrarDestino(SolicitudesViewModel _Destinos)
        {
            Solicitud objsolicitudes = new Solicitud();
            Destinos destinos = new Destinos();
            List<Solicitud> Sol = new List<Solicitud>();
            Sol = _SolicitudesData.ObtenerIdSolicitud().ToList();
            ViewBag.Sol = Convert.ToInt32(Sol[0].IdFolio) + 1;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {


                objsolicitudes.Folio = Convert.ToInt32(Sol[0].IdFolio) + 1;
                objsolicitudes.IdTipoSolicitud = _Destinos.Solicitud.IdTipoSolicitud;
                objsolicitudes.Departamento = _Destinos.Solicitud.Departamento;
                objsolicitudes.Empresa = _Destinos.Solicitud.Empresa;
                objsolicitudes.ImporteSolicitado = _Destinos.Solicitud.ImporteSolicitado;
                objsolicitudes.ImporteComprobado = _Destinos.Solicitud.ImporteComprobado;
                objsolicitudes.Estatus = _Destinos.Solicitud.Estatus;
                objsolicitudes.IdEstado = _Destinos.Solicitud.IdEstado;
                objsolicitudes.Id = _Destinos.Solicitud.Id;
                objsolicitudes.RFC = "456777";
                objsolicitudes.ClaveMoneda = _Destinos.Moneda.ClaveMoneda;
                //_SolicitudesData.InsertarSolicitud(objsolicitudes);
                ViewBag.Script = "Datos almacenados";

                destinos.ClavePais = _Destinos.Destino.ClavePais;
                destinos.IdEstado = _Destinos.Destino.IdEstado;
                destinos.IdCiudad = 1;
                destinos.Motivo = _Destinos.Destino.Motivo;
                destinos.FechaSalida = _Destinos.Destino.FechaSalida;
                destinos.FechaLlegada = _Destinos.Destino.FechaLlegada;
                destinos.Folio = _Destinos.Destino.Folio;
                //_DestinosData.InsertarDestino(destinos);


                var Pais = _UbicacionData.ObtenerPaises();
                var Estado = _UbicacionData.ObtenerEstados("MEX");
                var Ciudad = _UbicacionData.ObtenerCiudades("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Destino = _DestinosData.ObtenerDestinos(1);
                var SolicitudModel = new SolicitudesViewModel();

                SolicitudModel.Solicitudes = TipoSolicitud;
                SolicitudModel.Paises = Pais;
                SolicitudModel.Estados = Estado;
                SolicitudModel.Ciudades = Ciudad;
                SolicitudModel.Destinos = Destino;
                SolicitudModel.Monedas = _MonedaData.ObtenerMonedas();

                //var Pais = _UbicacionData.ObtenerPais();
                //var Estado = _UbicacionData.ObtenerEstado("MEX");
                //var Ciudad = _UbicacionData.ObtenerCiudad("MEX", 1);
                //var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                //var Destino = _DestinosData.ObtenerDestinos(1);
                 
                //SolicitudModel.Solicitudes = TipoSolicitud;
                //SolicitudModel.Paises = Pais;
                //SolicitudModel.Estados = Estado;
                //SolicitudModel.Ciudades = Ciudad;
                //SolicitudModel.Destinos = Destino;

                return Redirect("./Create");
            }
            catch (Exception)
            {

                throw;
            }
            //return Redirect("./ListarDestinos");
        }

        // GET: Solicitudes/Edit/5
        public ActionResult Edit(int Folio)
        {
             

            return View();

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