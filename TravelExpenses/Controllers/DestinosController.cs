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
    public class DestinosController : Controller
    {
        private readonly IDestinos _DestinosData;
        public DestinosController(IDestinos DestinosData)
        {
            this._DestinosData = DestinosData;
        }
        // GET: Destinos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Destinos/Details/5
        public ActionResult ListarDestinos()
        {
            var destinos = _DestinosData.ObtenerDestinos(1);
            var empresaModel = new DestinosViewModel();
            empresaModel.Destinos = destinos;

            return View(empresaModel);
             
        }

        // GET: Destinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarDestino(Destinos _Destinos)
        {
            Destinos destinos = new Destinos();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                destinos.ClavePais = _Destinos.ClavePais;
                destinos.IdEstado = _Destinos.IdEstado;
                destinos.IdCiudad = _Destinos.IdCiudad;
                destinos.Motivo = _Destinos.Motivo;
                destinos.FechaSalida = _Destinos.FechaSalida;
                destinos.FechaLlegada = _Destinos.FechaLlegada;
                destinos.Folio = _Destinos.Folio;
                //_DestinosData.InsertarDestino(destinos);
                
            }
            catch (Exception)
            {

                throw;
            }
            return Redirect("../Solicitudes/create");
        }

        // POST: Destinos/Create
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

        // GET: Destinos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Destinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Destinos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Destinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}