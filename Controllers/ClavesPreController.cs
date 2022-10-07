using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{

    [Authorize]

    public class ClavesPreController : Controller
    {
        private DBOperaciones repo;

        public ClavesPreController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index()
        {
            return View(repo.Getdosparam1<ClavePresupuestaria>("sp_clave_presupuestaria_lista", null).ToList());
        }


        [HttpGet]
        public IActionResult NuevaClave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NuevaClave(ClavePresupuestaria clavePresupuestaria)
        {
            clavePresupuestaria.accion = 1;
            repo.Getdosparam1<ClavePresupuestaria>("OBM_AgregaActualizaClavesPresupuestarias", clavePresupuestaria);
            return RedirectToAction("Index", "ClavesPre");
        }

        [HttpGet]
        public IActionResult EditarClave(int idArea)
        {
            return View(repo.Getdosparam1<ClavePresupuestaria>("obtenerOrgano", new { @idArea = idArea }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarClave(ClavePresupuestaria clavePresupuestaria)
        {
            clavePresupuestaria.accion = 2;
            repo.Getdosparam1<ClavePresupuestaria>("OBM_AgregaActualizaClavesPresupuestarias", clavePresupuestaria);
            return RedirectToAction("Index", "ClavesPre");
        }
    }
}
