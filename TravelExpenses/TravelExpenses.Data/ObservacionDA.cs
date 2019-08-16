using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.TravelExpenses.Data
{
    public class ObservacionDA : IObservacionDA
    {
        private readonly IConfiguration _configuration;
        private readonly TravelExpensesContext db;

        public ObservacionDA(TravelExpensesContext _db, IConfiguration configuration)
        {
            _configuration = configuration;
            this.db = _db;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("TravelExDb"));
            }
        }
        public int SolicitudesExportadas_ins(int Folio)
        {
            var list = new List<Empresas>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Folio", Folio);
                    var reader = Connection.Execute("SolicitudesExportadas_ins", queryParameters, commandType: CommandType.StoredProcedure);
                    return reader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Observacion ObtenerObservacion(int Folio)
        {
            var list = new List<Empresas>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Folio", Folio);
                    var reader = Connection.Query<Observacion>("Observacion_Obt", queryParameters, commandType: CommandType.StoredProcedure);
                    return reader.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerSolicitudes(string id)
        {

            try
            {
                var parameters = new DynamicParameters();

                using (IDbConnection conn = Connection)
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID", id);
                    var reader = Connection.Query<Solicitud>("SolicitudesObt_Centrocosto", queryParameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Folio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerSolicitudesFechas(string id,DateTime inicio,DateTime fin)
        {

            try
            {
                var parameters = new DynamicParameters();

                using (IDbConnection conn = Connection)
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID", id);
                    queryParameters.Add("@FechaInicio", inicio);
                    queryParameters.Add("@Fechafin", fin);
                    var reader = Connection.Query<Solicitud>("SolicitudesObt_CentrocostoFechas", queryParameters, commandType: CommandType.StoredProcedure);
                    return reader.OrderBy(x => x.Folio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus, string ID)
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

        public IEnumerable<Solicitud> ObtenerSolicitudesXEstatus(string Estatus, string ID,DateTime inicio , DateTime fin)
        {

            try
            {
                if (!string.IsNullOrEmpty(Estatus))
                {
                    return ObtenerSolicitudesFechas(ID, inicio,fin).Where(x => x.Estatus == Estatus);
                }
                else
                {
                    return ObtenerSolicitudesFechas(ID, inicio, fin);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Observacion Add(Observacion newRestaurant)
        {
            db.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public int Guardar(Observacion Observacion)
        {
            try
            {
                int result = 0;
                if (Observacion != null)
                {
                    if (Exists(Observacion.Folio))
                    {
                        db.Update(Observacion);
                    }
                    else
                    {
                        db.Add(Observacion);
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


        private bool Exists(int Folio)
        {
            return db.Observacion.Any(e => e.Folio == Folio);
        }
    }
}
