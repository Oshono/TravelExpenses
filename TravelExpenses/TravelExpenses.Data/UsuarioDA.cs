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
    public class UsuarioDA :IUsuario
    {
        private readonly TravelExpensesContext db;
        private readonly IConfiguration _configuration;
        public UsuarioDA(TravelExpensesContext db, IConfiguration configuration)
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
        public CorreosDestinatarios ObtenerCorreos (string username, string centroCosto, int? Folio)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", username);
                parameters.Add("@CentroCosto", centroCosto);
                parameters.Add("@FolioSolicitud", Folio);                
                using (IDbConnection conn = Connection)
                {
                    var result = Connection.Query<CorreosDestinatarios>("RolesCorreo_Obtener", parameters, commandType: CommandType.StoredProcedure);
 
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}

