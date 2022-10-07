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
    public class EstrategiaController : Controller
    {
        private DBOperaciones repo;

        public EstrategiaController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index()
        {
            return View(repo.Getdosparam1<Estrategia>("sp_indicador_estrategico_operaciones_obtener", new { @id = 0, @accion = 1 }).ToList());
        }

        [HttpGet]
        public IActionResult NuevaEstrategia()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NuevaEstrategia(Estrategia estrategia)
        {
            estrategia.accion = 1;
            repo.Getdosparam1<Estrategia>("sp_indicador_estrategico_agregar_actualizar", estrategia);
            return RedirectToAction("Index", "Estrategia");
        }

        [HttpGet]
        public IActionResult EditarEstrategia(int idIndEst)
        {
            return View(repo.Getdosparam1<Estrategia>("sp_indicador_estrategico_operaciones_obtener", new { @id = idIndEst, @accion = 2 }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarEstrategia(Estrategia estrategia)
        {
            estrategia.accion = 2;
            repo.Getdosparam1<Estrategia>("sp_indicador_estrategico_agregar_actualizar", estrategia);
            return RedirectToAction("Index", "Estrategia");
        }
    }
}
