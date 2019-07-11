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
    public class SolicitudesDA: ISolicitudes
    {
        private readonly IConfiguration _configuration;

        private readonly TravelExpensesContext db;
        public SolicitudesDA(TravelExpensesContext db, IConfiguration configuration)
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

        public int InsertarSolicitud(Solicitud solicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Folio", solicitud.Folio);
                parameters.Add("@IdTipoSolicitud", solicitud.IdTipoSolicitud);
                parameters.Add("@Departamento", solicitud.Departamento);
                parameters.Add("@Empresa", solicitud.Empresa); 
                parameters.Add("@ImporteSolicitado", solicitud.ImporteSolicitado);
                parameters.Add("@ImporteComprobado", solicitud.ImporteComprobado);
                parameters.Add("@Estatus", solicitud.Estatus); 
                parameters.Add("@IdEstado", solicitud.IdEstado);
                parameters.Add("@Id", solicitud.Id);
                parameters.Add("@RFC", solicitud.RFC);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Solicitudes_Ins", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<Solicitud> ObtenerTipos()
        { 
            try
            {
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Solicitud>("TipoSolicitud_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Descripcion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Solicitud ObtenerTipo()
        { 
            try
            {
                using (IDbConnection conn = connection)
                {
                    
                    var reader = connection.Query<Solicitud>("TipoSolicitud_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerSolicitudes()
        {
            
            try
            {

                var parameters = new DynamicParameters();
            
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Solicitud>("Solicitudes_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Folio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         
        //public IEnumerable<Solicitud> ObtenerSolicitudes()
        //{ 
        //    try
        //    {
        //        using (IDbConnection conn = connection)
        //        {
        //            var reader = connection.Query<Solicitud>("Solicitudes_Sel", null, commandType: CommandType.StoredProcedure);
        //            return reader.OrderBy(x => x.Descripcion);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public int InsertarDestino(Destinos destinos)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@IdDestinos", solicitud.IdDestinos);
                parameters.Add("@ClavePais", destinos.ClavePais);
                parameters.Add("@IdEstado", destinos.IdEstado);
                parameters.Add("@IdCiudad", destinos.IdCiudad);
                parameters.Add("@Motivo", destinos.Motivo);
                parameters.Add("@FechaSalida", destinos.FechaSalida);
                parameters.Add("@FechaLlegada", destinos.FechaLlegada);
                parameters.Add("@Folio", destinos.Folio);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Destinos_Ins", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerTipoSolicitud()
        {
            var list = new List<Solicitud>();
            try
            {
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Solicitud>("TipoSolicitud_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdTipoSolicitud);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public Solicitud Add(Solicitud newRestaurant)
        //{
        //    db.Add(newRestaurant);
        //    return newRestaurant;
        //}

        //public int Commit()
        //{
        //    return db.SaveChanges();
        //}

        //public int Guardar(Solicitud solicitud)
        //{
        //    try
        //    {
        //        int result = 0;
        //        if (solicitud != null)
        //        {
        //            if (Exists(solicitud.Folio))
        //            {
        //                db.Update(solicitud);
        //            }
        //            else
        //            {
        //                db.Add(solicitud);
        //            }

        //            result = Commit();

        //        }
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //private bool Exists(string Folio)
        //{
        //    return db.Solicitudes.Any(e => e.Folio == Folio);
        //}

    }
}
