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
    public class DepartamentoController : Controller
    {
        private readonly IDepartamento _departamento;
        
        public DepartamentoController(IDepartamento departamento)
        {
            _departamento = departamento;
        }

        // GET: Departamentos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var departamentos = _departamento.ObtenerDepartamentos();
            var departamentoModel = new DepartamentoViewModel();
            departamentoModel.Departamentos = departamentos;

            return View(departamentoModel);
        }

         
        // POST: Departamentos/Create
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

        // GET: Departamentos/Edit/5
        public ActionResult Edit(int idDepto)
        {
            var deptoModel = new DepartamentoViewModel();
            deptoModel.Departamento = new Departamentos();
            if (idDepto >0)
            {
                var depto = _departamento.ObtenerDepartamentos()
                            .Where( x => x.IdDepto == idDepto)
                            .FirstOrDefault();
                deptoModel.Departamento = depto;
            }
            return View(deptoModel);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartamentoViewModel depto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (depto.Departamento.IdDepto < 1)
            {
                return NotFound();
            }
            try
            {
                _departamento.Guardar(depto.Departamento);
            }
            catch
            {

            }

            return Redirect("/Departamento/Lista");
        }
              
    }
}