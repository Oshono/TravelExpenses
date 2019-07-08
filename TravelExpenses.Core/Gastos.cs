﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelExpenses.Core
{
    public class Gastos
    {
        public Gastos()
        {
            IdGasto = 0;
            Activo = true;
        }
        [Required, Key]
        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}