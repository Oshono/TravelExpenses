using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TravelExpenses.Controllers
{
    public class UsuariosRolesController : Controller
    {
        // GET: UsuariosRoles
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuariosRoles/Details/5
        [HttpGet]
        public string Prueba(string name)
        {
            return "Mamlon";
        }

        // GET: UsuariosRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosRoles/Create
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

        // GET: UsuariosRoles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuariosRoles/Edit/5
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

        // GET: UsuariosRoles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuariosRoles/Delete/5
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