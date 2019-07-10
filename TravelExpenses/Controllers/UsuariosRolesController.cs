using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelExpenses.Models;

namespace TravelExpenses.Controllers
{
    public class UsuariosRolesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        public UsuariosRolesController(UserManager<IdentityUser> userManager)
        {

        }
        // GET: UsuariosRoles
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuariosRoles/Details/5
        [HttpGet]
        [Authorize]
        public string Prueba(string name)
        {
            return "Esta Registrado";
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