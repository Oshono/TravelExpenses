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
    public class GastoDA : IGasto
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public GastoDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<Gastos> ObtenerGastos()
        {
            return db.CatGastos;
        }

        public IEnumerable<Gastos> ObtenerGastosPoliticas(string IDUser)
        {
            try
            {
                var parameters = new DynamicParameters();
                using (IDbConnection conn = Connection)
                {
                    parameters.Add("@ID", IDUser);
                    var reader = Connection.Query<Gastos>("CatGastosPoliticas_Obt", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdGasto);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int Guardar(Gastos Gasto)
        {
            try
            {
                int result = 0;
                if (Gasto != null)
                {
                    if (Exists(Gasto.IdGasto))
                    {
                        db.Update(Gasto);
                    }
                    else
                    {
                        db.Add(Gasto);
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

        private bool Exists(int Id)
        {
            return db.CatGastos.Any(e => e.IdGasto == Id);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}