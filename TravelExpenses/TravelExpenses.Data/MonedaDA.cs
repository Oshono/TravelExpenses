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
    public class MonedaDA : IMoneda
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public MonedaDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<Moneda> ObtenerMonedas()
        {
            return db.CatMonedas;
        }
        public int Guardar(Moneda moneda)
        {
            try
            {
                int result = 0;
                if (moneda != null)
                {
                    if (Exists(moneda.ClaveMoneda))
                    {
                        db.Update(moneda);
                    }
                    else
                    {
                        db.Add(moneda);
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

        private bool Exists(string ClaveMoneda)
        {
            return db.CatMonedas.Any(e => e.ClaveMoneda == ClaveMoneda);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}