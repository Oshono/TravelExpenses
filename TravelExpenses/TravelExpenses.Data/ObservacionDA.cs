﻿using System;
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
