using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class CatProdServSAT
    {
        public CatProdServSAT()
        {
            ClaveProdServ = string.Empty;
        }
        [Required, Key, StringLength(50)]
        public string ClaveProdServ { get; set; }
        [StringLength (50)]
        public string Descripcion { get; set; }
        
    }
}