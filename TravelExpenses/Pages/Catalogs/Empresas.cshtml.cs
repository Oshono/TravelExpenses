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
    public class EmpresasModel : PageModel
    {
        private readonly TravelExpensesContext _context;

        public EmpresasModel(TravelExpensesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Empresas Empresa { get; set; }
        public void OnGet()
        {

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