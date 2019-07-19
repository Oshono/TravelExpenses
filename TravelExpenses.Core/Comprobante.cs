using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Comprobante
    {
        public string Folio { get; set; }
        public string RFC { get; set; }
        public string NombreProveedor { get; set; }
        public string Fecha { get; set; }
        public float SubTotal { get; set; }
        public float Impuestos { get; set; }
        public float Total { get; set; }
        public string RegimenFiscal { get; set; }
        public string Moneda { get; set; }
        
    }
}