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
    public class PoliticaController : Controller
    {
        private readonly IPolitica _politica;
        private readonly IGasto _gastos;

        public PoliticaController(IPolitica politica, IGasto gasto)
        {
            _politica = politica;
            _gastos = gasto;
        }

        // GET: moneda Costos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var politicas = _politica.ObtenerPoliticas();
            var politicaModel = new PoliticaViewModel();
            politicaModel.Politicas = politicas;

            return View(politicaModel);
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
        public ActionResult Edit(int IdPolitica)
        {
            var politicaModel = new PoliticaViewModel();
            politicaModel.Politica = new Politica();

            politicaModel.Politica.Activo = true;
            if (IdPolitica > 0)
            {
                var politica = _politica.ObtenerPoliticas()
                            .Where(x => x.IdPolitica == IdPolitica)
                            .FirstOrDefault();
                politicaModel.Politica = politica;
            }
            var Gastos = _gastos.ObtenerGastos();
            politicaModel.Gastos = Gastos;
            return View(politicaModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PoliticaViewModel politicaModel)
        {
            if (!ModelState.IsValid)
            {
                return View(politicaModel);
            }             
            try
            {
                _politica.Guardar(politicaModel.Politica);
            }
            catch
            {

            }

            return Redirect("/Politica/Lista");
        }

    }
}