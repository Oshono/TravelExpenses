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
using System.Web;
using System.IO;

namespace TravelExpenses.Controllers
{
    public class ReembolsoController : Controller
    {
        private readonly ICentroCosto _centro;

        public ReembolsoController(ICentroCosto centro)
        {
            _centro = centro;
        }

        // GET: Centro Costos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var centros = _centro.ObtenerCentroCostos();
            var centroModel = new CentroCostoViewModel();
            centroModel.CentrosCostos = centros;

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
        public ActionResult Edit(string ClaveCentroCosto)
        {
            var centroModel = new CentroCostoViewModel();
            centroModel.CentroCosto = new CentroCosto();
            if (!string.IsNullOrEmpty(ClaveCentroCosto))
            {
                var centro = _centro.ObtenerCentroCostos()
                            .Where(x => x.ClaveCentroCosto == ClaveCentroCosto)
                            .FirstOrDefault();
                centroModel.CentroCosto = centro;
            }
            return View(centroModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CentroCostoViewModel centroCostoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }             
            try
            {
                _centro.Guardar(centroCostoModel.CentroCosto);
            }
            catch
            {

            }

            return Redirect("/CentroCosto/Lista");
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }

    }
}