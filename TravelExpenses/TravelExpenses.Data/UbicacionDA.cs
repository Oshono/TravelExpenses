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
    public class UbicacionDA :IUbicacion
    {
        private readonly TravelExpensesContext db;
        private readonly IConfiguration _configuration;
        public UbicacionDA(TravelExpensesContext db, IConfiguration configuration)
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
        public List<Paises> ObtenerPaises()
        {
            var list = new List<Paises>();
            try
            {
                return db.Paises.OrderBy(x=>x.Nombre).ToList();                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Estado> ObtenerEstados(string ClavePais)
        {
            var list = new List<Estado>();
            try
            {
                return db.Estados.Where(x => x.ClavePais == ClavePais).OrderBy(x=>x.Descripcion) .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Ciudades> ObtenerCiudades(string ClavePais, int? IdEstado)
        {
            var list = new List<Ciudades>();
            try
            {


                var ciudades = (from c in db.Ciudades
                                join e in db.Estados on c.IdEstado equals e.IdEstado
                                join p in db.Paises on e.ClavePais equals p.ClavePais
                                where (p.ClavePais == ClavePais || ClavePais == "")
                                select new
                                {
                                    c.IdCiudad,
                                    c.Activo,
                                    c.IdEstado,
                                    c.Descripcion,
                                    DescripcionEstado = e.Descripcion,
                                    p.ClavePais,
                                    NombrePais = p.Nombre
                                }).ToList();


                list = ciudades.ConvertAll(x => new Ciudades
                {
                    Descripcion = x.Descripcion,
                    IdCiudad = x.IdCiudad,
                    IdEstado = x.IdEstado,                    
                    Activo = x.Activo
                });
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public List<Destino> ObtenerDestinos(int? IdEstado, string ClavePais)
        {
            var list = new List<Destino>();
            try
            {
                

                var ciudades = (from c in db.Ciudades
                                join e in db.Estados on c.IdEstado equals e.IdEstado
                                join p in db.Paises on e.ClavePais equals p.ClavePais
                                where (p.ClavePais == ClavePais || ClavePais == "")
                                select new
                                {                                    
                                    c.IdCiudad,
                                    c.Activo,
                                    c.IdEstado,
                                    c.Descripcion,
                                    DescripcionEstado = e.Descripcion,
                                    p.ClavePais,
                                    NombrePais = p.Nombre
                                }).ToList();


                list = ciudades.ConvertAll(x => new Destino {
                    Ciudad = x.Descripcion,
                    IdCiudad = x.IdCiudad,
                    IdEstado = x.IdEstado,
                    DescripcionEstado = x.DescripcionEstado,
                    ClavePais = x.ClavePais,
                    NombrePais = x.NombrePais,
                    Activo = x.Activo
                });
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

