using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelExpenses.Core;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TravelExpenses.Data;

namespace TravelExpenses.Data
{
    public class SolicitudesDA: ISolicitudes
    {
        private readonly IConfiguration _configuration;
        public SolicitudesDA(IConfiguration configuration)
        {
            _configuration = configuration;
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
                parameters.Add("@TipoSolicitud", solicitud.TipoSolicitud);
                parameters.Add("@Departamento", solicitud.Departamento);
                parameters.Add("@Empresa", solicitud.Empresa);
                parameters.Add("@FechaSalida", solicitud.FechaSalida);
                parameters.Add("@FechaLlegada", solicitud.FechaLlegada);
                parameters.Add("@ImporteSolicitado", solicitud.ImporteSolicitado);
                parameters.Add("@ImporteComprobado", solicitud.ImporteComprobado);
                parameters.Add("@Estatus", solicitud.Estatus);
                parameters.Add("@Motivo", solicitud.Motivo);
                parameters.Add("@IdEstado", solicitud.IdEstado);
                parameters.Add("@IdUsuario", solicitud.IdUsuario);
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
    }
}
