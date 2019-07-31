using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Core
{
    public class Gasto
    {
        public int IdGastos { get; set; }
        public double MontoMaximo { get; set; }
        public double ImporteSolicitado { get; set; }
        public string TipoCambios { get; set; }
        public int Folio { get; set; }
        public string RFC { get; set; }
        public int IdGasto { get; set; }
        public string ClaveMoneda { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Empresa { get; set; }
    }
}
