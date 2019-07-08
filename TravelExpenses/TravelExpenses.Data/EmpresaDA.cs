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
    public class EmpresaDA : IEmpresa
    {
        private readonly TravelExpensesContext db;

        //public EmpresaDA(TravelExpensesContext db)
        //{
        //    this.db = db;
        //}

        private readonly IConfiguration _configuration;
        public EmpresaDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<Empresas> ObtenerEmpresas()
        {
            var list = new List<Empresas>();
            try
            {
                using (IDbConnection conn = Connection)
                {                    
                    var reader = Connection.Query<Empresas>("CatEmpresas_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Nombre);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Empresas ObtenerEmpresa(string RFC)
        {
            var list = new List<Empresas>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@RFC", RFC);
                    var reader = Connection.Query<Empresas>("CatEmpresas_Sel", queryParameters, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Empresas Add(Empresas newRestaurant)
        {
            db.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public int Guardar(Empresas Empresa)
        {
            try
            {
                int result = 0;
                if (Empresa != null)
                {
                    if (Exists(Empresa.RFC))
                    {
                        db.Update(Empresa);
                    }
                    else
                    {
                        db.Add(Empresa);
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

        private bool Exists(string RFC)
        {
            return db.CatEmpresas.Any(e => e.RFC == RFC);
        }
    }
}