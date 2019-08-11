using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Models;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;
namespace TravelExpenses.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISolicitudes _SolicitudesData;
        private readonly IDestinos _DestinosData;
        private readonly IUbicacion _UbicacionData;

        public HomeController(ISolicitudes SolicitudesData, IDestinos DestinosData, IUbicacion UbicacionData)
        {
            this._SolicitudesData = SolicitudesData;
            this._DestinosData = DestinosData;
            this._UbicacionData = UbicacionData;
        }
            //public IActionResult Index()
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Default()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SolicitudesEstatus(string estatus)
        {
            try
            {
                var SolicitudesModel = new SolicitudesViewModel();
                if (estatus == "Todo" || estatus == "--Seleccionar estatus")
                {
                    var Solicitud = _SolicitudesData.ObtenerSolicitudes();
                    SolicitudesModel.Solicitudes = Solicitud;
                    return Json(Solicitud);
                }
                else
                {
                    var Solicitud = _SolicitudesData.ObtenerSolicitudesEstatus(estatus);
                    SolicitudesModel.Solicitudes = Solicitud;
                    return Json(Solicitud);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public ActionResult ModificarEstatus(int Folio)
        {

            _SolicitudesData.ModificarEstatus(Folio);
            return Redirect("./");
        }

        public ActionResult EliminarSolicitud(int Folio)
        {
            _SolicitudesData.EliminarSolicitud(Folio);
            return Redirect("./");
        }
        [Authorize]
        public IActionResult ListarSolicitudes()
        {
            var SolicitudesModel = new SolicitudesViewModel();
            var Solicitud = _SolicitudesData.ObtenerSolicitudes();
            var Estatus = _SolicitudesData.EstatusSolicitudes();


            
            SolicitudesModel.Estatuses = Estatus;

            SolicitudesModel.Solicitudes = Solicitud; 

            return View(SolicitudesModel);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
