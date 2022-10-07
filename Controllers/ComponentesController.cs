using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Models;
using PoaPlan.Models.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{
    [Authorize]
    public class ComponentesController : Controller
    {
        private DBOperaciones repo;

        public ComponentesController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index(int anio = 0)
        {
            if(anio == 0)
            {
                return View();
            }
            else
            {
                ViewBag.elAnio = anio;
                return View(repo.Getdosparam1<ComponenteLista>("sp_componente_obtener_componente_anio", new { @anio = anio }).ToList());
            }
        }
    
        [HttpGet]
        public IActionResult NuevoComponente(int elAnioC)
        {
            ViewBag.losOrganosAdmivos = repo.Getdosparam1<ClavePresupuestaria>("sp_organos_por_anio", new { @anio = elAnioC}).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult NuevoComponente(Componente elcomponente)
        {
            elcomponente.accion = 1;
            repo.Getdosparam1<Componente>("AgregaActualizaComponentes", elcomponente);
            return RedirectToAction("Index", "Componentes");
        }

        [HttpGet]
        public IActionResult EditarComponente(int elAnioC, int idComp)
        {
            ViewBag.losOrganosAdmivos = repo.Getdosparam1<ClavePresupuestaria>("sp_organos_por_anio", new { @anio = elAnioC }).ToList();
            return View(repo.Getdosparam1<Componente>("sp_componente_especifico", new { @idComponente = idComp }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarComponente(Componente componenteEditar)
        {
            componenteEditar.accion = 2;
            repo.Getdosparam1<Componente>("AgregaActualizaComponentes", componenteEditar);
            return RedirectToAction("Index", "Componentes");
        }
    }
}
