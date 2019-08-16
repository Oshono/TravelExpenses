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
using System.Security.Claims;
using TravelExpenses.Services;

namespace TravelExpenses.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISolicitudes _SolicitudesData;
        private readonly IDestinos _DestinosData;
        private readonly IUbicacion _UbicacionData;
        private readonly ICentroCosto _centroCosto;
        private readonly IUsuario Usuario;
        private readonly IEmailSender Email;
        private readonly IPolitica _Politica;
        public HomeController(ISolicitudes SolicitudesData,
            IDestinos DestinosData,
            IUbicacion UbicacionData,
            ICentroCosto centroCosto,
            IUsuario usuario,
            IEmailSender email,
            IPolitica _politica)
        {
            this._SolicitudesData = SolicitudesData;
            this._DestinosData = DestinosData;
            this._UbicacionData = UbicacionData;
            this._centroCosto = centroCosto;
            this.Usuario = usuario;
            this.Email = email;
            this._Politica = _politica;
        }
            //public IActionResult Index()
        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Default", "Home");
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
            //SendEmail(Correos.Solicitante, subject, message, Correos.Procesador);

            return true;
        }

        public Task SendEmail(string email, string subject, string message, string CC)
        {
            return Email.SendEmailCCAsync(email, CC, subject, message);
        }

        public IActionResult SolicitudesEstatus(string estatus)
        {
            try
            {
                var SolicitudesModel = new SolicitudesViewModel();
                if (estatus == "Todo" || estatus == "--Seleccionar estatus")
                {
                    var Solicitud = _SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                     SolicitudesModel.Solicitudes = Solicitud;
                    return Json(Solicitud);
                }
                else
                {
                    var Solicitud = _SolicitudesData.ObtenerSolicitudesEstatus(estatus,User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    SolicitudesModel.Solicitudes = Solicitud;
                    return Json(Solicitud);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        
         [HttpPost]
        public ActionResult ModificarEstatus(int Folio)
        {

            try
            {

                _SolicitudesData.ModificarEstatus(Folio);
                ObtenerCorreos(Folio, "Solicitud de Autorización Solicitud " + Folio.ToString(), "Se solicita autorización para la solicitud con Folio: " + Folio.ToString());
                return Redirect("./ListarSolicitudes");
            }
            catch (Exception ex)
            {

                return Redirect("./ListarSolicitudes");
            }
        }

        public ActionResult EliminarSolicitud(int Folio)
        {
            _SolicitudesData.EliminarSolicitud(Folio);
            return Redirect("./ListarSolicitudes");
        }
        [Authorize]
        public IActionResult ListarSolicitudes()
        {
            var validar = _SolicitudesData.ValidarSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();
            int result = validar[0].result;
            if (result == 0)
            {
                ViewBag.Enviar = 0;
            }

            var politicas = _Politica.ObtenerPoliticas().ToList();
            var SolicitudesModel = new SolicitudesViewModel();
            var Solicitud = _SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
