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
        private readonly TravelExpensesContext _context;

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

        // GET: Departamentos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Departamentos/Edit/5
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

        // GET: Departamentos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Departamentos/Delete/5
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