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
    public class CentroCostoDA : ICentroCosto
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public CentroCostoDA(TravelExpensesContext db, IConfiguration configuration)
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
        
        public IEnumerable<CentroCosto> ObtenerCentroCostos()
        {
            return db.CatCentroCostos;
        }
        public int Guardar(CentroCosto centro)
        {
            try
            {
                int result = 0;
                if (centro != null)
                {
                    if (Exists(centro.ClaveCentroCosto))
                    {
                        db.Update(centro);
                    }
                    else
                    {
                        db.Add(centro );
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
        public int GuardarCentroConstosUsuario(CentroCostoUsuario centroCostoUsuario)
        {
            try
            {
                int result = 0;
                if (centroCostoUsuario != null)
                {
                    if (ExistsUser(centroCostoUsuario.ClaveCentroCosto,centroCostoUsuario.Id))
                    {
                        db.Update(centroCostoUsuario);
                    }
                    else
                    {
                        db.Add(centroCostoUsuario);
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

        private bool Exists(string ClaveCentroCosto)
        {
            return db.CatCentroCostos.Any(e => e.ClaveCentroCosto == ClaveCentroCosto);
        }
        private bool ExistsUser(string ClaveCentroCosto,string Id)
        {
            return db.CatCentroCostoUsuario.Any(e => e.ClaveCentroCosto == ClaveCentroCosto && e.Id==Id);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}