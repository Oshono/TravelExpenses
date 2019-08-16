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
    public class PoliticaController : Controller
    {
        private readonly IPolitica _politica;
        private readonly IGasto _gastos;
        private readonly IPoliticaDetalle _polDetalle;
        private readonly ICentroCosto _centroCosto;

        public PoliticaController(  IPolitica politica, 
                                    IGasto gasto, 
                                    IPoliticaDetalle poldetalle,
                                    ICentroCosto centroCosto
                                   )
        {
            _politica = politica;
            _gastos = gasto;
            _polDetalle = poldetalle;
            _centroCosto = centroCosto;
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
            var politicas = _politica.ObtenerPoliticas();
            var politicaModel = new PoliticaViewModel();
            politicaModel.Politicas = politicas;

            return View(politicaModel);
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
        public ActionResult Edit(int IdPolitica)
        {
            var politicaModel = new PoliticaViewModel();
            politicaModel.Politica = new Politica();
            
            politicaModel.Politica.Activo = true;
            var Gastos = _gastos.ObtenerGastos();
            var CentrosCosto = _centroCosto.ObtenerCentroCostos();
            if (IdPolitica > 0)
            {
                var politica = _politica.ObtenerPoliticas()
                            .Where(x => x.IdPolitica == IdPolitica)
                            .FirstOrDefault();
                politicaModel.Politica = politica;
                
                politica.Detalle = _polDetalle.ObtenerDetalles(IdPolitica);
                foreach (PoliticaDetalle polDetalle in politica.Detalle)
                {
                    polDetalle.DescripcionGasto = Gastos.Where(x => x.IdGasto == polDetalle.IdGasto).FirstOrDefault().Nombre;
                }
                if (politica.Detalle==null || politica.Detalle.Count <1)
                {
                    politica.Detalle = new List<PoliticaDetalle>();
                }

            }
            
            politicaModel.Gastos = Gastos;
            politicaModel.CentroCostos = CentrosCosto;
            return View(politicaModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PoliticaViewModel politicaModel)
        {
                   
            try
            {
                var ImportePermitido = politicaModel.Politica.ImportePermitido;
                var IdGasto = politicaModel.Politica.IdGasto;
                politicaModel.Politica.IdGasto = 0;
                politicaModel.Politica.ImportePermitido = 0;
                _politica.Guardar(politicaModel.Politica);
                if (IdGasto > 0)
                {
                    PoliticaDetalle pDetalle = new PoliticaDetalle();
                    pDetalle.IdGasto = IdGasto;
                    pDetalle.IdPolitica = politicaModel.Politica.IdPolitica;
                    pDetalle.ImportePermitido = ImportePermitido;
                    pDetalle.Activo = true;
                    _polDetalle.Guardar(pDetalle);                    
                }
            }
            catch (Exception ex)
            {

                return View();
            }

            return RedirectToAction("Edit", "Politica", new { IdPolitica = politicaModel.Politica.IdPolitica });
        }

    }
}