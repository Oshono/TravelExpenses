using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelExpenses.Core
{
    public class CentroCostoUsuario
    {
        public CentroCostoUsuario()
        {
            ClaveCentroCosto = string.Empty;
            Id = string.Empty;
        }
        [Required, Key, StringLength(20)]
        public string ClaveCentroCosto { get; set; }
        [StringLength (450)]
        public string Id { get; set; }
       
    }
}