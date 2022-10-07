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
    public class ActividadesController : Controller
    {
        private DBOperaciones repo;

        public ActividadesController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index(int elComponente = 0)
        {
            ViewBag.ComboAnioArea = repo.Getdosparam1<ClavePresupuestaria>("sp_actividades_lista_anio_organo", null).ToList();

            if (elComponente == 0)
            {
                return View();
            }
            else
            {
                //llevar los datos de las actividades
                ViewBag.idC = elComponente;
                return View(repo.Getdosparam1<Indicador>("sp_actividades_obtener_componente_idComponente", new { @idComponente = elComponente }).ToList());
            }           
        }

        public JsonResult GetComponentes(int elIdArea)
        {
            return Json(repo.Getdosparam1<Componente>("sp_actividades_obtener_componente_idArea", new { @idArea = elIdArea }).ToList());
        }

        [HttpGet]
        public IActionResult NuevaActividad(int elIdComp)
        {
            ViewBag.idCompoPasado = elIdComp;
            return View();
        }

        [HttpPost]
        public IActionResult NuevaActividad(Indicador newIndicador)
        {
            newIndicador.accion = 1;
            repo.Getdosparam1<Indicador>("AgregaActualizaActividades", newIndicador);
            return RedirectToAction("Index", "Actividades");
        }

        [HttpGet]
        public IActionResult EditarActividad(int elIdcompEd)
        {
            return View(repo.Getdosparam1<Indicador>("sp_actividades_obtener_actividad_por_indicador", new { @idIndicador = elIdcompEd }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarActividad(Indicador updIndicador)
        {
            updIndicador.accion = 2;
            repo.Getdosparam1<Indicador>("AgregaActualizaActividades", updIndicador);
            return RedirectToAction("Index", "Actividades");
        }
    }
}
