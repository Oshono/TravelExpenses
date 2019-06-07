using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelExpenses.WebApi.Controllers
{
    [Route("Api/[Controller]")]
    public class SesionController : Controller
    {
        [HttpGet]
        public Boolean Inicio(string Usuario,string Contraseña)
        {
            return true;
        }
        [HttpPost]
        public Boolean Cerrar()
        {
            return true;
        }
    }
}
