using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly ISolicitudes _SolicitudesData;

        public IEnumerable<Solicitud> Solicitudes { get; set; }

        [BindProperty]
        public Solicitud Solicitud { get; set; }

        public IndexModel (ISolicitudes solicitudes)
        {
            this._SolicitudesData = solicitudes;
        }

        public void OnGet()
        {
            Solicitudes = _SolicitudesData.ObtenerSolicitudes(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}