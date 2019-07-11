using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{
    public class DestinoController : Controller
    {
        private readonly IUbicacion _ubicacion;

        public DestinoController(IUbicacion ubicacion)
        {
            _ubicacion = ubicacion;
        }

        // GET: Centro Costos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var ciudades = new List<Destino>();
            var paises = _ubicacion.ObtenerPaises();
            var ubicacionModel = new DestinoViewModel();
            ubicacionModel.Destinos = ciudades;
            ubicacionModel.Paises = paises;
            return View(ubicacionModel);
        }

        [HttpPost]
        public ActionResult CargaCiudades(string ClavePais)
        {
            var ciudades = _ubicacion.ObtenerCiudades(0, ClavePais);            
            return Json(ciudades);
        }

        // POST: gastos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: gastos/Edit/5
        public ActionResult Edit(string ClaveCentroCosto)
        {
            //var centroModel = new CentroCostoViewModel();
            //centroModel.CentroCosto = new CentroCosto();
            //if (!string.IsNullOrEmpty(ClaveCentroCosto))
            //{
            //    var centro = _ubicacion.ObtenerCentroCostos()
            //                .Where(x => x.ClaveCentroCosto == ClaveCentroCosto)
            //                .FirstOrDefault();
            //    centroModel.CentroCosto = centro;
            //}
            return View();
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CentroCostoViewModel centroCostoModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}             
            //try
            //{
            //    _ubicacion.Guardar(centroCostoModel.CentroCosto);
            //}
            //catch
            //{

            //}

            return Redirect("/CentroCosto/Lista");
        }

    }
}