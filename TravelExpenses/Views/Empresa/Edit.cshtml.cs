using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Pages.Catalogs
{
    public class AddEmpresasModel : PageModel
    {
        private readonly IEmpresa empresa;
        private readonly TravelExpensesContext _context;

        public AddEmpresasModel(TravelExpensesContext context, IEmpresa miempresa)
        {
            _context = context;
            this.empresa = miempresa;
        }

        [BindProperty]
        public Empresas Empresa { get; set; }
        public IActionResult OnGet()
        {
            //if (RFC !="")
            //{
            //    Empresa = empresa.ObtenerEmpresa(RFC);
            //}
            //else
            //{
            //    Empresa = new Empresas();
            //}
            //if (Empresa == null)
            //{
            //    return RedirectToPage("./NotFound");
            //}
            return Page();
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