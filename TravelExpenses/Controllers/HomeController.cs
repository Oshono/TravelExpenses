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
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SolicitudesEstatus(string estatus)
        {
            var Solicitud = _SolicitudesData.ObtenerSolicitudesEstatus(estatus);
            var SolicitudesModel = new SolicitudesViewModel();
            SolicitudesModel.Solicitudes = Solicitud;

            return View(SolicitudesModel);
        }

        public ActionResult ModificarEstatus(int Folio)
        {

            _SolicitudesData.ModificarEstatus(Folio);
            return Redirect("./");
        }

        public IActionResult ListarSolicitudes()
        {
 

            var Solicitud = _SolicitudesData.ObtenerSolicitudes();
            var SolicitudesModel = new SolicitudesViewModel();
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
