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

        public SolicitudesController(ISolicitudes SolicitudesData, IDestinos DestinosData, IUbicacion UbicacionData)
        {
            this._SolicitudesData = SolicitudesData;
            this._DestinosData = DestinosData;
            this._UbicacionData = UbicacionData;
        }
        // GET: Solicitudes
        public ActionResult Index()
        {
            return View();
          
        }

        // GET: Solicitudes/Details/5
        public ActionResult ListarDestinos()
        {
            var destinos = _DestinosData.ObtenerDestinos("13");
            var empresaModel = new DestinosViewModel();
            empresaModel.Destinos = destinos;
            
            return View(empresaModel);
        }

        // GET: Solicitudes/Create
        public ActionResult Create()
        {
            var Pais = _UbicacionData.ObtenerPais();
            var Estado = _UbicacionData.ObtenerEstado("MEX");
            var Ciudad = _UbicacionData.ObtenerCiudad("MEX", 1);
            var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
            //var Destino = _DestinosData.ObtenerDestinos("13");

            var SolicitudModel = new SolicitudesViewModel();
            SolicitudModel.Solicitudes = TipoSolicitud;
            SolicitudModel.Paises = Pais;
            SolicitudModel.Estados = Estado;
            SolicitudModel.Ciudades = Ciudad;
            //SolicitudModel.Destinos = Destino;
            return View(SolicitudModel);
           
        }
        public ActionResult ListarPaises()
        {
            var Pais = _UbicacionData.ObtenerPais();
            var PaisesModel = new SolicitudesViewModel();
            PaisesModel.Paises = Pais;
            return View(PaisesModel);
        }

        // POST: Solicitudes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudesViewModel _solicitudes)
        {
            int count = 0;

            _DestinosData.ObtenerDestino("13");
            var result = _DestinosData.ObtenerDestinos("13");
            Solicitud objsolicitudes = new Solicitud();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                count++;
                objsolicitudes.Folio = "F_"+count.ToString();
                objsolicitudes.IdTipoSolicitud = _solicitudes.Solicitud.IdTipoSolicitud;
                objsolicitudes.Departamento = _solicitudes.Solicitud.Departamento;
                objsolicitudes.Empresa = _solicitudes.Solicitud.Empresa;
                objsolicitudes.ImporteSolicitado = _solicitudes.Solicitud.ImporteSolicitado;
                objsolicitudes.ImporteComprobado = _solicitudes.Solicitud.ImporteComprobado;
                objsolicitudes.Estatus = _solicitudes.Solicitud.Estatus;
                objsolicitudes.Estatus = _solicitudes.Solicitud.Estatus;
                objsolicitudes.IdEstado = _solicitudes.Solicitud.IdEstado;
                objsolicitudes.Id = _solicitudes.Solicitud.Id;
                objsolicitudes.RFC = _solicitudes.Solicitud.RFC;

                ViewBag.Script = "Datos almacenados";
                var Pais = _UbicacionData.ObtenerPais();
                var Estado = _UbicacionData.ObtenerEstado("MEX");
                var Ciudad = _UbicacionData.ObtenerCiudad("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Destino = _DestinosData.ObtenerDestinos("13");

                var SolicitudModel = new SolicitudesViewModel();
                SolicitudModel.Solicitudes = TipoSolicitud;
                SolicitudModel.Paises = Pais;
                SolicitudModel.Estados = Estado;
                SolicitudModel.Ciudades = Ciudad;
                SolicitudModel.Destinos = Destino;

                ViewBag.Folio = objsolicitudes.Folio;
                return View(SolicitudModel);

            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public ActionResult RegistrarDestino(DestinosViewModel _Destinos)
        {
            Destinos destinos = new Destinos();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                destinos.ClavePais ="MEX";
                destinos.IdEstado = 1;
                destinos.IdCiudad = 1;
                destinos.Motivo = _Destinos.Destino.Motivo;
                destinos.FechaSalida = _Destinos.Destino.FechaSalida;
                destinos.FechaLlegada = _Destinos.Destino.FechaLlegada;
                destinos.Folio = _Destinos.Destino.Folio;
                _DestinosData.InsertarDestino(destinos);


                var Pais = _UbicacionData.ObtenerPais();
                var Estado = _UbicacionData.ObtenerEstado("MEX");
                var Ciudad = _UbicacionData.ObtenerCiudad("MEX", 1);
                var TipoSolicitud = _SolicitudesData.ObtenerTipoSolicitud();
                var Destino = _DestinosData.ObtenerDestinos("13");

                var SolicitudModel = new SolicitudesViewModel();
                SolicitudModel.Solicitudes = TipoSolicitud;
                SolicitudModel.Paises = Pais;
                SolicitudModel.Estados = Estado;
                SolicitudModel.Ciudades = Ciudad;
                SolicitudModel.Destinos = Destino;

                return View(SolicitudModel);
            }
            catch (Exception)
            {

                throw;
            }
            //return Redirect("./ListarDestinos");
        }

        // GET: Solicitudes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Solicitudes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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