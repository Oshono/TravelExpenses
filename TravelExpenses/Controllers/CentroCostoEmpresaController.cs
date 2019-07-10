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
    public class CentroCostoEmpresaController : Controller
    {
        private readonly ICentroCostoEmpresa _centroEmpresa;

        public CentroCostoEmpresaController(ICentroCostoEmpresa centroEmpresa)
        {
            _centroEmpresa = centroEmpresa;
        }

        // GET: Centro Costos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var centros = _centroEmpresa.ObtenerCentroCostosEmpresa();
            var centroModel = new CentroCostoEmpresaViewModel();
            var empresas = _centroEmpresa.ObtenerEmpresas();
            centroModel.CentroCostosEmpresas = centros;
            centroModel.Empresas = empresas;
            centroModel.CentroCostos = _centroEmpresa.ObtenerCentrosCostos().ToList();
            return View(centroModel);
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
        public ActionResult Edit(string ClaveCentroCosto, string RFC)
        {
            var centroModel = new CentroCostoEmpresaViewModel();
            centroModel.CentroCostoEmpresa = new CentroCostoEmpresa();
            if (!string.IsNullOrEmpty(ClaveCentroCosto))
            {
                var centro = _centroEmpresa.ObtenerCentroCostosEmpresa()
                            .Where(x => x.ClaveCentroCosto == ClaveCentroCosto)
                            .FirstOrDefault();
                centroModel.CentroCostoEmpresa = centro;
            }
            return View(centroModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CentroCostoEmpresaViewModel centroCostoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }             
            try
            {
                foreach (CentroCosto centro in centroCostoModel.CentroCostos.Where(x => x.checkboxAnswer == true).ToList())
                {
                    centroCostoModel.CentroCostoEmpresa = new CentroCostoEmpresa();
                    centroCostoModel.CentroCostoEmpresa.RFC = centroCostoModel.Empresa;
                    centroCostoModel.CentroCostoEmpresa.ClaveCentroCosto = centro.ClaveCentroCosto;
                    _centroEmpresa.Guardar(centroCostoModel.CentroCostoEmpresa);
                }
            }
            catch
            {

            }

            return Redirect("/CentroCostoEmpresa/Lista");
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(CentroCostoEmpresaViewModel centroCostoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _centroEmpresa.Guardar(centroCostoModel.CentroCostoEmpresa);
            }
            catch
            {

            }

            return Redirect("/CentroCostoEmpresa/Lista");
        }
    }
}