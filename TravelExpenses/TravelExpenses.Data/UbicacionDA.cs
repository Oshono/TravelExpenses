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
        private readonly IConfiguration _configuration;
        public UbicacionDA(IConfiguration configuration)
        {
            _configuration = configuration;
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
                using (IDbConnection conn = Connection)
                {
                    var reader = Connection.Query<Paises>("Paises_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Nombre).AsList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Estado> ObtenerEstados()
        {
            var list = new List<Estado>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var reader = Connection.Query<Estado>("Estado_Sel", null, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.NombreEstado).AsList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public List<Ciudades> ObtenerCiudades(int? IdEstado)
        {
            var list = new List<Ciudades>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IdEstado", IdEstado, DbType.String, ParameterDirection.Input);

                using (IDbConnection conn = Connection)
                {
                    var reader = Connection.Query<Ciudades, Estado, Ciudades>(
                        "Ciudad_Sel",
                        (ciudad, estado) =>
                        {
                            ciudad.Estado = estado;
                            return ciudad;
                        },
                        splitOn: "IdEstado",
                        param: parameters,
                        commandType: CommandType.StoredProcedure
                        );
                    return reader.OrderBy(x => x.Ciudad).AsList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
