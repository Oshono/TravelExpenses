using System.Collections.Generic;
using TravelExpenses.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TravelExpenses.Data
{
    public class SqlEstadoData : IEstado
    {
        private readonly TravelExpensesContext db;

        public SqlEstadoData(TravelExpensesContext db)
        {
            this.db = db;
        }

        public Estado GetEstado(int id)
        {
            return db.Estados.Find(id);
        }

        public IEnumerable<Estado> GetEstados(string name)
        {
            var query = from r in db.Estados
                        where r.Descripcion.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Descripcion
                        select r;
            return query;
        }
        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}
