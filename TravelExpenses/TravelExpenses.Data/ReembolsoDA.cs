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
    public class ReembolsoDA: IReembolso
    {
        private readonly IConfiguration _configuration;

        private readonly TravelExpensesContext db;
        public ReembolsoDA(TravelExpensesContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            this.db = db;
        }
        public IDbConnection connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("TravelExDb"));
            }
        }
        public IEnumerable<Archivo> ObtenerArchivos()
        {
            return db.Archivos;
        }
        public int Guardar(Archivo archivo)
        {
            try
            {
                int result = 0;
                if (archivo != null)
                {
                    if (Exists(archivo.IdArchivo))
                    {
                        db.Update(archivo);
                    }
                    else
                    {
                        db.Add(archivo);
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
        private bool Exists(int IdArchivo)
        {
            return db.Archivos.Any(e => e.IdArchivo == IdArchivo);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }

    }
}
