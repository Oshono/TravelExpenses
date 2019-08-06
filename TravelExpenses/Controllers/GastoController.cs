using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{
    public class GastoController : Controller
    {
        private readonly IGasto _gasto;

        public GastoController(IGasto gasto)
        {
            _gasto = gasto;
        }

        // GET: Gasto
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Lista()
        {
            var gastos = _gasto.ObtenerGastos();
            var gastoModel = new GastoViewModel();
            gastoModel.Gastos = gastos;

            return View(gastoModel);
        }


        // POST: gastos/Create
        [HttpPost]
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int idGasto)
        {
            var gastoModel = new GastoViewModel();
            gastoModel.Gasto = new Gastos();
            if (idGasto > 0)
            {
                var gasto = _gasto.ObtenerGastos()
                            .Where(x => x.IdGasto == idGasto)
                            .FirstOrDefault();
                gastoModel.Gasto = gasto;
            }
            return View(gastoModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GastoViewModel gastomdl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }             
            try
            {
                _gasto.Guardar(gastomdl.Gasto);
            }
            catch
            {

            }

            return Redirect("/Gasto/Lista");
        }

    }
}