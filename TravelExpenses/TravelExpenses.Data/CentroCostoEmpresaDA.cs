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
    public class CentroCostoEmpresaDA : ICentroCostoEmpresa
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public CentroCostoEmpresaDA(TravelExpensesContext db, IConfiguration configuration)
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
        public IEnumerable<CentroCostoEmpresa> ObtenerCentroCostosEmpresa()
        {
            return db.CatCentroCosto_Empresa;
        }
        public IEnumerable<CentroCosto> ObtenerCentrosCostos()
        {
            return db.CatCentroCostos;
        }
        public IEnumerable<Empresas> ObtenerEmpresas()
        {
            return db.CatEmpresas;
        }
        public int Guardar(CentroCostoEmpresa centroEmpresa)
        {
            try
            {
                int result = 0;
                if (centroEmpresa != null)
                {
                    if (!Exists(centroEmpresa.ClaveCentroCosto, centroEmpresa.RFC))
                    {
                        db.Add(centroEmpresa);
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

        private bool Exists(string ClaveCentroCosto, string RFC)
        {
            return db.CatCentroCosto_Empresa.Any(e => e.ClaveCentroCosto == ClaveCentroCosto && e.RFC == RFC);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}