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
    public class AvanceActividadController : Controller
    {
        private DBOperaciones repo;

        public AvanceActividadController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index(int elIndicador = 0)
        {
            ViewBag.ComboAnioArea = repo.Getdosparam1<ClavePresupuestaria>("sp_actividades_lista_anio_organo", null).ToList();

            if(elIndicador == 0)
            {
                return View();
            }
            else
            {
                ViewBag.elIdIndica = elIndicador;
                //return View(repo.Getdosparam1<AvanceActividad>("sp_avance_actividad_obtener_por_indicador", new { @elIndicador = elIndicador }).FirstOrDefault());
                return View(repo.Getdosparam1<AvanceActividad>("sp_avance_actividad_obtener_por_indicador", new { @elIndicador = elIndicador }).ToList());
            }
            
        }

        public JsonResult GetIndicador(int elIdComp)
        {
            return Json(repo.Getdosparam1<Indicador>("sp_actividades_obtener_componente_idComponente", new { @idComponente = elIdComp }).ToList());
        }

        [HttpGet]
        public IActionResult NuevoAvanceActividad(int elIdAvAc)
        {
            ViewBag.elIdIndica = elIdAvAc;
            return View();
        }

        [HttpPost]
        public IActionResult NuevoAvanceActividad(AvanceActividad avanceActividad)
        {           
            avanceActividad.accion = 1;
            repo.Getdosparam1<AvanceActividad>("AgregaActializaAvanceActividad", avanceActividad);
            return RedirectToAction("Index", "AvanceActividad");
        }

        [HttpGet]
        public IActionResult EditarAvanceActividad(int elIndicadorEditar)
        {
            return View(repo.Getdosparam1<AvanceActividad>("sp_avance_actividad_obtner_por_idActividad", new { @idActividad= elIndicadorEditar }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarAvanceActividad(AvanceActividad avanceActividad)
        {
            avanceActividad.accion = 2;
            repo.Getdosparam1<AvanceActividad>("AgregaActializaAvanceActividad", avanceActividad);
            return RedirectToAction("Index", "AvanceActividad");
        }
    }
}
