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
    public class DepartamentoDA : IDepartamento
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public DepartamentoDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<Departamentos> ObtenerDepartamentos()
        {
            return db.CatDepartamentos;
        }
        public int Guardar(Departamentos Depto)
        {
            try
            {
                int result = 0;
                if (Depto != null)
                {
                    if (Exists(Depto.IdDepto))
                    {
                        db.Update(Depto);
                    }
                    else
                    {
                        db.Add(Depto);
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

        private bool Exists(int IdDepto)
        {
            return db.CatDepartamentos.Any(e => e.IdDepto == IdDepto);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}