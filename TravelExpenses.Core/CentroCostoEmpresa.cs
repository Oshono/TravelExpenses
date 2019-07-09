using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class CentroCostoEmpresa
    {
        public CentroCostoEmpresa()
        {
            ClaveCentroCosto = string.Empty;
            RFC = string.Empty;
        }
        [Required, Key, StringLength(20)]
        public string ClaveCentroCosto { get; set; }
        [Required, Key, StringLength(13)]
        [RegularExpression("^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([A-Z]|[0-9]){2}([A]|[0-9]){1})?$", ErrorMessage = "RFC invalido")]
        public string RFC { get; set; }
    }
}