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
    public class PoliticaDetalleDA : IPoliticaDetalle
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public PoliticaDetalleDA(TravelExpensesContext db, IConfiguration configuration)
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
        public List<PoliticaDetalle> ObtenerDetalles(int IdPolitica)
        {
            return ObtenerDetalles().Where(x=>x.IdPolitica == IdPolitica).ToList();
        }
        public List<PoliticaDetalle> ObtenerDetalles()
        {
            return db.PoliticaDetalle.ToList();
        }
        public int Guardar(PoliticaDetalle detalle)
        {
            try
            {
                int result = 0;
                if (detalle != null)
                {
                    if (Exists(detalle.IdPolitica, detalle.IdGasto))
                    {
                        db.PoliticaDetalle.Update(detalle);
                    }
                    else
                    {
                        db.PoliticaDetalle.Add(detalle);
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

        private bool Exists(int IdPolitica, int IdGasto)
        {
            return db.PoliticaDetalle.Any(e => e.IdPolitica == IdPolitica && e.IdGasto == IdGasto);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

        
    }
}