using TravelExpenses.Core;
using Microsoft.EntityFrameworkCore;

namespace TravelExpenses.Data
{
    public class TravelExpensesContext: DbContext
    {
        public TravelExpensesContext(DbContextOptions<TravelExpensesContext> options)
            : base(options)
        {

        }

        public DbSet<Estado> Estados { get; set; }
    }
}
