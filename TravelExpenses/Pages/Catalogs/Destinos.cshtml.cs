using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;


namespace TravelExpenses.Pages.Catalogs
{
    public class CiudadesModel : PageModel
    {
        private readonly IUbicacion Ubicacion;
        public IEnumerable<Ciudades> Ciudades { get; set; }
        public IEnumerable<Estado> Estados { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        //[BindProperty(SupportsGet = true)]

        [BindProperty]
        public Ciudades Ciudad { get; set; }

        public string SearchTerm { get; set; }
        public CiudadesModel(IUbicacion ubicacionData)
        {
            this.Ubicacion = ubicacionData;
        }
        public void OnGet()
        {
            Ciudades = Ubicacion.ObtenerCiudades(0);
            Paises = Ubicacion.ObtenerPaises();
            Estados = Ubicacion.ObtenerEstados("MEX");
        }
         

    }
}