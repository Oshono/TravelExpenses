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
    public class MonedaController : Controller
    {
        private readonly IMoneda _moneda;

        public MonedaController(IMoneda moneda)
        {
            _moneda = moneda;
        }

        // GET: moneda Costos
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Lista()
        {
            var monedas = _moneda.ObtenerMonedas();
            var monedaModel = new MonedaViewModel();
            monedaModel.Monedas = monedas;

            return View(monedaModel);
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
        public ActionResult Edit(string ClaveMoneda)
        {
            var monedaModel = new MonedaViewModel();
            monedaModel.moneda = new Moneda();

            if (!string.IsNullOrEmpty(ClaveMoneda))
            {
                var moneda = _moneda.ObtenerMonedas()
                            .Where(x => x.ClaveMoneda == ClaveMoneda)
                            .FirstOrDefault();
                monedaModel.moneda = moneda;
            }
            return View(monedaModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MonedaViewModel MonedaModel)
        {
            if (!ModelState.IsValid)
            {
                return View(MonedaModel);
            }             
            try
            {
                _moneda.Guardar(MonedaModel.moneda);
            }
            catch
            {

            }

            return Redirect("/Moneda/Lista");
        }

    }
}