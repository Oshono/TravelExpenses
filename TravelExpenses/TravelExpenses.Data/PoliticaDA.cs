using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelExpenses.Core;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace TravelExpenses.Data
{
    public class PoliticaDA : IPolitica
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public PoliticaDA(TravelExpensesContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            this.db = db;
        }
        
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("TravelExDb"));
            }
        }
        public IEnumerable<Politica> ObtenerPoliticas()
        {
            return db.Politica;
        }
        public int Guardar(Politica politica)
        {
            try
            {
                int result = 0;
                if (politica != null)
                {
                    if (Exists(politica.IdPolitica))
                    {
                        db.Update(politica);
                    }
                    else
                    {
                        db.Add(politica);
                    }

                    result = Commit();

                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Exists(int IdPolitica)
        {
            return db.Politica.Any(e => e.IdPolitica == IdPolitica);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}