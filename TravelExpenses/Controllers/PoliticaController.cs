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
        private readonly IPoliticaDetalle _polDetalle;

        public PoliticaController(  IPolitica politica, 
                                    IGasto gasto, 
                                    IPoliticaDetalle poldetalle
                                   )
        {
            _politica = politica;
            _gastos = gasto;
            _polDetalle = poldetalle;
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
                politica.Detalle = _polDetalle.ObtenerDetalles(IdPolitica);
                if (politica.Detalle==null || politica.Detalle.Count <1)
                {
                    politica.Detalle = new List<PoliticaDetalle>();
                }

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
                if (politicaModel.Politica.IdGasto > 0)
                {
                    PoliticaDetalle pDetalle = new PoliticaDetalle();
                    pDetalle.IdGasto = politicaModel.Politica.IdGasto;
                    pDetalle.IdPolitica = politicaModel.Politica.IdPolitica;
                    pDetalle.ImportePermitido = politicaModel.Politica.ImportePermitido;
                    pDetalle.Activo = true;
                    _polDetalle.Guardar(pDetalle);
                }
            }
            catch
            {

            }

            return RedirectToAction("Edit", "Politica", new { IdPolitica = politicaModel.Politica.IdPolitica });
        }

    }
}