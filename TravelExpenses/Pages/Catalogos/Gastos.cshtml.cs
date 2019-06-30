using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Pages.Catalogs
{
    public class GastosModel : PageModel
    {
        private readonly TravelExpensesContext _context;

        public GastosModel(TravelExpensesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Gastos Gasto { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CatGastos.Add(Gasto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}