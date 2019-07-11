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
    public class DestinosDA: IDestinos
    {
        private readonly IConfiguration _configuration;

        private readonly TravelExpensesContext db;
        public DestinosDA(TravelExpensesContext db, IConfiguration configuration)
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


        public IEnumerable<Destinos> ObtenerDestinos(string Folio)
        {
            var list = new List<Destinos>();
            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("@Folio", Folio);
                using (IDbConnection conn = connection)
                {
                    var reader = connection.Query<Destinos>("Destinos_Sel", parameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.IdDestinos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Destinos ObtenerDestino(string Folio)
        {
            var lista = new List<Destinos>();
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@IdDestinos", solicitud.IdDestinos);
                parameters.Add("@Folio", Folio);
                using (IDbConnection conn = connection)
                {
                    var result = connection.Query<Destinos>("Destinos_Sel", parameters, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault(); ;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int InsertarDestino(Destinos Destino)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClavePais", Destino.ClavePais);
                parameters.Add("@IdEstado", Destino.IdEstado);
                parameters.Add("@IdCiudad", Destino.IdCiudad);
                parameters.Add("@Motivo", Destino.Motivo);
                parameters.Add("@FechaSalida", Destino.FechaSalida);
                parameters.Add("@FechaLlegada", Destino.FechaLlegada);
                parameters.Add("@Folio", Destino.Folio); 
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
    }
}
