using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class CorreosDestinatarios
    {
        public CorreosDestinatarios()
        {
        
        }
        public string Solicitante { get; set; }
        public string Aprobador { get; set; }
        public string Procesador { get; set; }

        
    }
}