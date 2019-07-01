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
        public Departamentos Add(Departamentos newDepartamento)
        {
            db.Add(newDepartamento);
            return newDepartamento;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}