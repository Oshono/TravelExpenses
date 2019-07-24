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
    public class RembolsoDA: IRembolso
    {
        private readonly IConfiguration _configuration;
        private readonly IComprobante _comprobante;

        private readonly TravelExpensesContext db;
        public RembolsoDA(TravelExpensesContext db, IConfiguration configuration, IComprobante comprobante)
        {
            _configuration = configuration;
            _comprobante = comprobante;

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
        public int Guardar(Comprobante miComprobante)
        {
            try
            {
                int result = 0;
                if (miComprobante != null)
                {
                    if (Exists(miComprobante.Archivo.NombreArchivo, miComprobante.Archivo.Extension))
                    {
                       
                    }
                    else
                    {
                        _comprobante.Guardar(miComprobante);

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

        public int GuardarComprobante(Comprobante miComprobante)
        {
            try
            {
                int result = 0;
                if (miComprobante != null)
                {
                    _comprobante.Guardar(miComprobante);
                    result = Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exists(string NombreArchivo, string Extension)
        {
            return db.Archivos.Any(e => e.NombreArchivo == NombreArchivo && e.Extension == Extension);
        }
        public bool Exists(string UUID)
        {
            return db.Comprobante.Any(e => e.UUID == UUID);
        }
        public int Commit()
        {
            return db.SaveChanges();
        }       

    }
}
