﻿using System;
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
    public class EmpresaController : Controller
    {
        private readonly IEmpresa _empresa;
        private readonly TravelExpensesContext _context;

        public EmpresaController(IEmpresa empresa)
        {
            _empresa = empresa;
        }
        // GET: Empresa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaEmpresas()
        {
            var empresas = _empresa.ObtenerEmpresas();
            var empresaModel = new EmpresaViewModel();
            empresaModel.Empresas = empresas;

            return View(empresaModel);
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empresa/Create
        public ActionResult AddEmpresas()
        {
            return View();
        }

        // POST: Empresa/Create
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

        // GET: Empresa/Edit/RFC
        public ActionResult Edit(string RFC)
        {
            var empresaModel = new EmpresaViewModel();
            if (RFC != "")
            { 
                var empresa = _empresa.ObtenerEmpresa(RFC);                
                empresaModel.Empresa = empresa;
            }
            return View(empresaModel);
        }
        
        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public   ActionResult Edit(EmpresaViewModel miempresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (miempresa.Empresa.RFC == "")
            {
                return NotFound();
            }
            try
            {
                _empresa.Guardar(miempresa.Empresa);
            }
            catch  
            {
               
            }

            return Redirect("/Empresa/ListaEmpresas");
        }

        // GET: Empresa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Empresa/Delete/5
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