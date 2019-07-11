using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class CentroCostoEmpresaViewModel
    {
        public IEnumerable<CentroCostoEmpresa> CentroCostosEmpresas { get; set; }
        public IEnumerable<Empresas> Empresas { get; set; }
        public List<CentroCosto> CentroCostos { get; set; }
        public CentroCostoEmpresa CentroCostoEmpresa { get; set; }
        public string Empresa { get; set; }
    }
}
