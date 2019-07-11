using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;

namespace TravelExpenses.Pages.Requests
{
    public class SolicitudesModel : PageModel
    {
        private readonly ISolicitudes _solicitudes;
        private readonly IUbicacion Ubicacion;
        private readonly IDepartamento _Departamento;

        public IEnumerable<Ciudades> Ciudades { get; set; }
        public IEnumerable<Estado> Estados { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IEnumerable<Destinos> _Solicitudes { get; set; }
        public IEnumerable<Departamentos> Deptos { get; set; }


        [BindProperty]
        public Solicitud Solicitudes { get; set; }
        public Ciudades Ciudad { get; set; }
        public Departamentos _Deptos { get; set; }

        public string SearchTerm { get; set; }
        public SolicitudesModel(ISolicitudes solicitudes,IUbicacion ubicacionData,IDepartamento Departamento)
        {
            this._solicitudes = solicitudes;
            this.Ubicacion = ubicacionData;
            this._Departamento = Departamento;
        }
        public void OnPost()
        {
            //InsertarSolicitudes();
            //InsertarDestino();

        }
        //public int InsertarSolicitudes()
        //{
        //    //Solicitud solicitudes = new Solicitud();
        //    //solicitudes.Folio = "";
        //    //solicitudes.TipoSolicitud = 1;
        //    //solicitudes.Departamento = "";
        //    //solicitudes.Empresa = "";
        //    ////solicitudes.FechaSalida = DateTime.Now;
        //    ////solicitudes.FechaLlegada = DateTime.Now;
        //    //solicitudes.ImporteSolicitado = 1;
        //    //solicitudes.ImporteComprobado = 2;
        //    //solicitudes.Estatus = "";
        //    ////solicitudes.Motivo = "";
        //    //solicitudes.IdEstado = 1;
        //    ////solicitudes.IdUsuario = 1;

        //    //var result = _solicitudes.InsertarSolicitud(solicitudes);
        //    //return result;
             
        //}

        //public int InsertarDestino()
        //{
        //    //Destinos destinos = new Destinos();
        //    //destinos.ClavePais = "MEX";
        //    //destinos.IdDestinos = 1;
        //    //destinos.IdEstado=1;
        //    //destinos.IdCiudad = 1;
        //    //destinos.Motivo = "";
        //    //destinos.FechaSalida=DateTime.Now;
        //    //destinos.FechaLlegada = DateTime.Now;
        //    //destinos.IdSolicitudes = 1; 

        //    //var result = _solicitudes.InsertarDestino(destinos);
        //    //return result;

        //}

        public void OnGet()
        {
            Ciudades = Ubicacion.ObtenerCiudades(0);
            Paises = Ubicacion.ObtenerPaises();
            Estados = Ubicacion.ObtenerEstados("MEX");
            //_Solicitudes = _solicitudes.ObtenerDestinos(1);
            //Deptos = _Departamento.ObtenerDepartamentos();



        }
    }
}