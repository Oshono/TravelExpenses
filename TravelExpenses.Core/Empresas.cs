﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Empresas
    {
        public Empresas()
        {
            FechaAlta = DateTime.Now;
            Activo = true;
        }
        [Required, Key, StringLength(13)]
        [RegularExpression("^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([A-Z]|[0-9]){2}([A]|[0-9]){1})?$", ErrorMessage = "RFC invalido")]
        public string RFC { get; set; }
        public string Nombre { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
    }
}