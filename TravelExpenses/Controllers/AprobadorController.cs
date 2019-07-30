using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelExpenses.Controllers
{
    public class AprobadorController : Controller
    {
        private readonly ISolicitudes SolicitudesData;

        public AprobadorController(ISolicitudes solicitudes)
        {
            SolicitudesData = solicitudes;
        }


        // GET: /<controller>/

        public ActionResult AprobarSolicitud()
        {
           

            SolicitudesViewModel solicitud = new SolicitudesViewModel();
            solicitud.Solicitudes = SolicitudesData.ObtenerSolicitudesXEstatus("PorComprobar");

            return View(solicitud);
        }
    }
}
