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
                //parameters.Add("@Folio", solicitud.Folio);
                parameters.Add("@IdTipoSolicitud", solicitud.IdTipoSolicitud);
                parameters.Add("@Departamento", solicitud.Departamento);
                parameters.Add("@Empresa", solicitud.Empresa); 
                parameters.Add("@ImporteSolicitado", solicitud.ImporteSolicitado);
                parameters.Add("@ImporteComprobado", solicitud.ImporteComprobado);
                parameters.Add("@Estatus", solicitud.Estatus); 
                //parameters.Add("@IdEstado", solicitud.IdEstado);
                parameters.Add("@Id", solicitud.Id);
                parameters.Add("@RFC", solicitud.RFC);
                //parameters.Add("@ClaveMoneda", solicitud.ClaveMoneda);
                using (IDbConnection conn = connection)
                {
                    var result = connection.ExecuteScalar("Solicitudes_Ins", parameters, commandType: CommandType.StoredProcedure).ToString();
                    return int.Parse(result);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarComentarios(Comentarios comentarios)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Folio", comentarios.Folio);
                parameters.Add("@Comentario", comentarios.Comentario);

                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Comentarios_Ins", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int EliminarSolicitud(int Folio)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("Folio", Folio);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Solicitudes_Eli", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int ModificarEstatus(int Folio)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Folio",Folio);
                 
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("ModificarEstatus", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int ActualizarEstatus(int Folio, string Estatus)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Folio", Folio);
                parameters.Add("@Estatus", Estatus);

                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("ActualizarEstatus", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<Solicitud> ObtenerSolicitudes(string ID)
        {
            
            try
            {
                var parameters = new DynamicParameters();
            
                using (IDbConnection conn = connection)
                {
                    parameters.Add("@ID", ID);
                    var reader = connection.Query<Solicitud>("Solicitudes_Sel", parameters, null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Folio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerSolicitudesEstatus(string estatus,string ID)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Estatus", estatus);
                parameters.Add("ID", ID);
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Solicitud>("FiltroEstatus", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Folio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Estatus> EstatusSolicitudes()
        {
            try
            {
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Estatus>("Estatus_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Status);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Gasto> ObtenerGastos(int Folio)
        {
            var list = new List<Destinos>();
            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("@Folio", Folio);
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Gasto>("Gastos_Sel", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdGastos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Comentarios> ObtenerComentario(int Folio)
        { 
            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("@Folio", Folio);
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Comentarios>("Comentarios_Sel", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdComentarios);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus,string ID)
        {

            try
            {
                if (!string.IsNullOrEmpty(Estatus))
                {
                    return ObtenerSolicitudes(ID).Where(x => x.Estatus == Estatus);
                }
                else
                {
                    return ObtenerSolicitudes(ID);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         
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
                //parameters.Add("@Folio", destinos.Folio);
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

        public IEnumerable<Solicitud> ObtenerIdSolicitud()
        {
            var list = new List<Solicitud>();
            try
            {
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Solicitud>("ObtenerUltimo", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdTipoSolicitud);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertarGastos(Gasto gasto)
        {
            try
            {
                var parameters = new DynamicParameters();
                
                parameters.Add("@MontoMaximo", gasto.MontoMaximo);
                parameters.Add("@ImporteSolicitado", gasto.ImporteSolicitado);
                parameters.Add("@TipoCambios", gasto.TipoCambios);
                parameters.Add("@Folio", gasto.Folio);
                parameters.Add("@RFC", gasto.RFC);
                parameters.Add("@IdGasto", gasto.IdGasto);
                parameters.Add("@ClaveMoneda", gasto.ClaveMoneda);
                
                
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Gastos_Ins", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Solicitud SolicitudesXFolio(int Folio)
        {
            try
            {
                var parameters = new DynamicParameters();
                using(IDbConnection conn = connection)
                {
                    parameters.Add("@Folio", Folio);
                    var reader = connection.Query<Solicitud>("Solicitudes_SelFolio", parameters, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Destinos> DestinosXFolio(int Folio)
        {
            try
            {
                var parameters = new DynamicParameters();
                using (IDbConnection conn = connection)
                {
                    parameters.Add("@Folio", Folio);
                    var reader = connection.Query<Destinos>("Destinos_Sel", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdDestinos);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public int Destinos_Upd(Destinos _destinos)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdDestinos", _destinos.IdDestinos);
                parameters.Add("@ClavePais", _destinos.ClavePais);
                parameters.Add("@IdEstado", _destinos.IdEstado);
                parameters.Add("@IdCiudad", _destinos.IdCiudad);
                parameters.Add("@Motivo  ", _destinos.Motivo);
                parameters.Add("@FechaSalida", _destinos.FechaSalida);
                parameters.Add("@FechaLlegada", _destinos.FechaLlegada);
                using(IDbConnection conn = connection)
                {
                    var result = conn.Execute("Destinos_upd", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }


        public int Gastos_Upd(Gasto _Gastos)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdGastos", _Gastos.IdGastos); 
                parameters.Add("@ImporteSolicitado", _Gastos.ImporteSolicitado);
                parameters.Add("@TipoCambios", _Gastos.TipoCambios); 
                parameters.Add("@IdGasto", _Gastos.IdGasto);
                parameters.Add("@ClaveMoneda", _Gastos.ClaveMoneda);
                using (IDbConnection conn = connection)
                {
                    var result = conn.Execute("Gastos_upd", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Destinos ObtenerDestinosFolio(int IdDestinos)
        {
            try
            {
                var parameters = new DynamicParameters();
                using (IDbConnection conn = connection)
                {
                    parameters.Add("@IdDestinos", IdDestinos);
                    var reader = connection.Query<Destinos>("Destinos_SelFolio", parameters, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Gasto ObtenerGastosFolio(int IdGastos)
        {
            try
            {
                var parameters = new DynamicParameters();
                using (IDbConnection conn = connection)
                {
                    parameters.Add("@IdGastos  ", IdGastos);
                    var reader = connection.Query<Gasto>("Gastos_SelFolio", parameters, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int EliminarDestinos(int IdDestinos)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdDestinos", IdDestinos);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Destinos_Eli", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int EliminarGastos(int IdGastos)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdGastos", IdGastos);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Gastos_Eli", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int SolicitudesUpd(Solicitud solicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Folio", solicitud.Folio);
                parameters.Add("@IdTipoSolicitud", solicitud.IdTipoSolicitud);
                parameters.Add("@ImporteSolicitado", solicitud.ImporteSolicitado);
                parameters.Add("@ClaveMoneda", solicitud.ClaveMoneda);

                using (IDbConnection conn = connection)
                {
                    var result = connection.Execute("Solicitudes_Upd", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Politicas> ObtenerPoliticas(string ID)
        { 
            try
            {
                var parameters = new DynamicParameters();
                using(IDbConnection conn = connection)
                {
                    parameters.Add("@ID", ID);
                    var reader = connection.Query<Politicas>("CatGastosPoliticas_Obt", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x=>x.IdGasto);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
