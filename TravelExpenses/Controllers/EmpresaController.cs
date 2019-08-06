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
    public class EmpresaController : Controller
    {
        private readonly IEmpresa _empresa;

        public EmpresaController(IEmpresa empresa)
        {
            _empresa = empresa;
        }
        // GET: Empresa
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult ListaEmpresas()
        {
            var empresas = _empresa.ObtenerEmpresas();
            var empresaModel = new EmpresaViewModel();
            empresaModel.Empresas = empresas;

            return View(empresaModel);
        }
         
        // POST: Empresa/Create
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

        // GET: Empresa/Edit/RFC
        [Authorize]
        public ActionResult Edit(string RFC)
        {
            var empresaModel = new EmpresaViewModel();
            empresaModel.Empresa = new Empresas();
            if (!string.IsNullOrEmpty(RFC))
            {
                var empresa = _empresa.ObtenerEmpresa(RFC);
                empresaModel.Empresa = empresa;
            }
            return View(empresaModel);
        }
        
        // POST: Empresa/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public   ActionResult Edit(EmpresaViewModel miempresa)
        {
            if (!ModelState.IsValid)
            {
                return View(miempresa);
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
    }
}