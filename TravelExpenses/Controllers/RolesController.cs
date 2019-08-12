using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using TravelExpenses.Services;
using TravelExpenses.Core;

namespace TravelExpenses.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ICentroCosto _centro;

        public RolesController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
             IEmailSender emailSender,
             ICentroCosto centro)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
            _centro = centro;
        }

        public IActionResult Index(string Mesaje)
        {
            var roles = _context.Roles.ToList();
            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();
            var centroCostos = _centro.ObtenerCentroCostos();

            IEnumerable<CentroCosto> costos = centroCostos;

           var convertedUsers = users.Select(x => new UsersViewModel
            {
                Email = x.Email,
                Roles = roles
                    .Where(y => userRoles.Any(z => z.UserId == x.Id && z.RoleId == y.Id))
                    .Select(y => new UsersRole
                    {
                        Name = y.NormalizedName
                    })
            });

           //DisplayViewModel Model = new DisplayViewModel();
           // Model.CentroCostos=new IEnumerable<CentroCosto>({ });


            return View(new DisplayViewModel
            {
                Roles = roles.Select(x => x.NormalizedName),
                Users = convertedUsers,
                CentroCostos= costos,
                Mesaje=Mesaje//.Select(x=>x.Nombre)
            });
        }

 
        public IActionResult ConsultarPermisos(string Email,string Id)
        {
            var roles = _context.Roles.ToList();
            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();
            

            var centroCostos = _centro.ObtenerCentroCostos();

            IEnumerable<CentroCosto> costos = centroCostos;

            var convertedUsers = users.Select(x => new UsersViewModel
            {
                Email = x.Email,
                Roles = roles
                    .Where(y => userRoles.Any(z => z.UserId == x.Id && z.RoleId == y.Id))
                    .Select(y => new UsersRole
                    {
                        Name = y.NormalizedName
                    })
            });

            var CentroCostoAsociado = _centro.ConsultarControCostoPorUsuario(Id).First();
          

            return View("ConsultarPermisos",new DisplayViewModel
            {
                Roles = roles.Select(x => x.NormalizedName),
                CentroCostos = costos.Where(x=>x.ClaveCentroCosto== CentroCostoAsociado.ClaveCentroCosto),
                Users = convertedUsers.Where(x => x.Email == Email)
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(string email)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            await _userManager.CreateAsync(user, "password");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel vm)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = vm.Name });
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.UserEmail);
            var CostosUsuario = new CentroCostoUsuario();

            if (vm.Delete)
            {
                await _userManager.RemoveFromRoleAsync(user, vm.Role);
                //await _userManager.DeleteAsync(user);
                return RedirectToAction("Index", new { Mesaje = "Eliminacion de Role "+vm.Role });
            }
                
            if (vm.DeleteUser)
            {
                await _userManager.DeleteAsync(user);
                //await _userManager.SetLockoutEnabledAsync(user, true);
                //await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);
                return RedirectToAction("Index", new { Mesaje = "Eliminacion de Role " + vm.Role + " al usuario" + user.UserName });
            }
                
            if(vm.Role!=null && vm.ClaveCentroCosto!= "Selecciona un Centro de Costos")
            {
                CostosUsuario.ClaveCentroCosto = vm.ClaveCentroCosto;
                CostosUsuario.Id = user.Id;
                await _userManager.AddToRoleAsync(user, vm.Role);
                _centro.GuardarCentroConstosUsuario(CostosUsuario);
                await _emailSender.SendEmailAsync(vm.UserEmail, "Permisos otorgados.",
                       $"Se te ha otorgado permisos de "+ vm.Role);

                return RedirectToAction("Index", new { Mesaje = "Exitoso Role agragado a "+user.UserName+"Asociado al centro de costo "+vm.ClaveCentroCosto });
            }

            if (vm.Consultar)
            {
                //this.ConsultarPermisos(vm.UserEmail);
                return RedirectToAction("ConsultarPermisos","Roles", new { Email = vm.UserEmail,Id=user.Id })
;            }
            else
                return RedirectToAction("Index",new  { Mesaje="Selesione una opcion valida"});

           
        }
    }

    public class DisplayViewModel
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<UsersViewModel> Users { get; set; }
        public IEnumerable<CentroCosto> CentroCostos { get; set; }
        public string Mesaje { get; set; }

    }

    public class UsersViewModel
    {
        public string Email { get; set; }
        public IEnumerable<UsersRole> Roles { get; set; }
        public string ClaveCentroCosto { get; set; }
    }

    public class UsersRole
    {
        public string Name { get; set; }
    }

    public class RoleViewModel
    {
        public string Name { get; set; }
    }

    public class UpdateUserRoleViewModel
    {
        public IEnumerable<UsersViewModel> Users { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<CentroCosto> CentroCostos { get; set; }

        public string UserEmail { get; set; }
        public string Role { get; set; }
        public bool Delete { get; set; }
        public bool DeleteUser { get; set; }
        public bool Consultar { get; set; }
        public string ClaveCentroCosto { get; set; }
    }
}