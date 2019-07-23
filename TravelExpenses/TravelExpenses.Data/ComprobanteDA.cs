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
    public class ComprobanteDA : IComprobante
    {
        private readonly TravelExpensesContext db;

        private readonly IConfiguration _configuration;
        public ComprobanteDA(TravelExpensesContext db, IConfiguration configuration)
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
        
        public IEnumerable<Comprobante> ObtenerComprobantes()
        {
            return db.Comprobante;
        }
        public Comprobante ObtenerComprobantesXID(string UUID)
        {
            var comprobante =  db.Comprobante.Where(x=>x.UUID == UUID).FirstOrDefault();
            comprobante.Archivo = db.Archivos.Where(x=>x.UUID == UUID).FirstOrDefault();
            comprobante.Conceptos = db.Concepto.Where(x => x.UUID == UUID).ToList();

            return comprobante;
        }
        public int Guardar(Comprobante comprobante)
        {
            try
            {
                int result = 0;
                if (comprobante != null)
                {
                    if (Exists(comprobante.UUID))
                    {
                        db.Comprobante.Update(comprobante);
                    }
                    else
                    {
                        db.Comprobante.Add(comprobante);
                        db.Concepto.AddRange(comprobante.Conceptos);
                        db.Archivos.Add(comprobante.Archivo);
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

        private bool Exists(string UUID)
        {
            return db.Comprobante.Any(e => e.UUID == UUID);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }


    }
}