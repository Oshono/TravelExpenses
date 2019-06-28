using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Pages.Requests
{
    public class SolicitudesModel : PageModel
    {
        private readonly ISolicitudes _solicitudes;
        [BindProperty]
        public Solicitud Solicitudes { get; set; }

        public string SearchTerm { get; set; }
        public SolicitudesModel(ISolicitudes solicitudes)
        {
            this._solicitudes = solicitudes;
        }
        public void OnPost()
        {
            InsertarSolicitudes(); 
        }
        public int InsertarSolicitudes()
        {
            Solicitud solicitudes = new Solicitud();
            solicitudes.Folio = "";
            solicitudes.TipoSolicitud = "";
            solicitudes.Departamento = "";
            solicitudes.Empresa = "";
            solicitudes.FechaSalida = DateTime.Now;
            solicitudes.FechaLlegada = DateTime.Now;
            solicitudes.ImporteSolicitado = 1;
            solicitudes.ImporteComprobado = 2;
            solicitudes.Estatus = "";
            solicitudes.Motivo = "";
            solicitudes.IdEstado = 1;
            solicitudes.IdUsuario = 1;

            var result = _solicitudes.InsertarSolicitud(solicitudes);
            return result;
             
        }
       
        public void OnGet()
        {
        }
    }
}