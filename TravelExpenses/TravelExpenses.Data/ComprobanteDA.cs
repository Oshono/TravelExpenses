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
        public Comprobante ObtenerComprobantesXID(string UUID)
        {
            var catProdServ = _prodserv.ObtenerCatalogo();

            var comprobante =  db.Comprobante.Where(x=>x.UUID == UUID).FirstOrDefault();
            comprobante.Archivo = db.Archivos.Where(x=>x.UUID == UUID).FirstOrDefault();
            comprobante.Conceptos = db.Concepto.Where(x => x.UUID == UUID).ToList();

            //   concepto.DescripcionProdServ = catProdServ.Where(x => x.ClaveProdServ == conceptoXML.Attribute("ClaveProdServ").Value).Select(c => c.Descripcion).FirstOrDefault().ToString();
            foreach (var concepto in comprobante.Conceptos)
            {
                var catprodserv = catProdServ.FirstOrDefault(x => x.ClaveProdServ == concepto.ClaveProdServ);

                if (catprodserv != null)
                {
                    concepto.DescripcionProdServ = catprodserv.Descripcion;
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