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
    public class FormaPagoDA : IFormaPago
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public FormaPagoDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<FormaPago> ObtenerFormasPago()
        {
            return db.CatFormaPago;
        }
  
        
    }
}