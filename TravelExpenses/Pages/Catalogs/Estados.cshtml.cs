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
    public class EstadosModel : PageModel
    {
        private readonly IEstado estadoData;
        public IEnumerable<Estado> Estados { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public EstadosModel(IEstado estadoData)
        {
            this.estadoData = estadoData;
        }
        public void OnGet()
        {
            Estados = estadoData.GetEstados(SearchTerm);
        }
    }
}