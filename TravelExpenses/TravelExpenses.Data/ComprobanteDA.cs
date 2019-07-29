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
        private readonly ICatProdServSATDA _prodserv;
        private readonly IConfiguration _configuration;
        public ComprobanteDA(TravelExpensesContext db, IConfiguration configuration, ICatProdServSATDA prodserv)
        {
            _configuration = configuration;
            _prodserv = prodserv;
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
        public List<Comprobante> ObtenerComprobantes(int FolioSolicitud)
        {
            var comprobantes = db.Comprobante.Where(x => x.FolioSolicitud == FolioSolicitud).ToList();
            foreach (Comprobante comprobante in comprobantes)
            {
                comprobante.Archivos = db.Archivos.Where(x => x.UUID == comprobante.UUID).ToList();
                comprobante.Conceptos = db.Concepto.Where(x => x.UUID == comprobante.UUID).ToList();
            }
            return comprobantes;
        }

        public Comprobante ObtenerComprobantesXID(string UUID)
        {
            var catProdServ = _prodserv.ObtenerCatalogo();

            var comprobante =  db.Comprobante.Where(x=>x.UUID == UUID).FirstOrDefault();
            comprobante.Archivos = db.Archivos.Where(x=>x.UUID == UUID).ToList();
            comprobante.Conceptos = db.Concepto.Where(x => x.UUID == UUID).ToList();

            if (comprobante.Conceptos.Count() > 0)
            {
                foreach (var concepto in comprobante.Conceptos)
                {
                    var catprodserv = catProdServ.FirstOrDefault(x => x.ClaveProdServ == concepto.ClaveProdServ);

                    if (catprodserv != null)
                    {
                        concepto.DescripcionProdServ = catprodserv.Descripcion;
                    }
                }
            }
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
                        if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0)
                        { 
                            db.Concepto.AddRange(comprobante.Conceptos);
                        }
                        db.Archivos.AddRange(comprobante.Archivos);
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
        public   void Delete(string UUID)
        {
                if (Exists(UUID))
                {
                    var Archivos = db.Archivos.Where(x => x.UUID == UUID).ToList();
                    db.Archivos.RemoveRange(Archivos);

                    var Conceptos = db.Concepto.Where(x => x.UUID == UUID).ToList();
                    db.Concepto.RemoveRange(Conceptos);

                    var comprobante = ObtenerComprobantesXID(UUID);
                    db.Comprobante.Remove(comprobante);
                    Commit();
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