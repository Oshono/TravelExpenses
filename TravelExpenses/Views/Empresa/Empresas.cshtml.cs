using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Pages.Catalogs
{
    public class EmpresasModel : PageModel
    {
        private readonly IEmpresa empresa;
        private readonly TravelExpensesContext _context;
        public IEnumerable<Empresas> Empresas { get; set; }

        public EmpresasModel(TravelExpensesContext context, IEmpresa miempresa)
        {
            _context = context;
            this.empresa = miempresa;
        }

        [BindProperty]
        public Empresas Empresa { get; set; }
        public void OnGet()
        {
            Empresas = empresa.ObtenerEmpresas();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CatEmpresas.Add(Empresa);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}