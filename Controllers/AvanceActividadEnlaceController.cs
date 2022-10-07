using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Helper;
using PoaPlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{
    [Authorize]

    public class AvanceActividadEnlaceController : Controller
    {
        private DBOperaciones repo;

        public AvanceActividadEnlaceController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Enlace")]
        public IActionResult Index(int elIndicador = 0)
        {
            ViewBag.elOrgano = repo.Getdosparam1<ClavePresupuestaria>("obtenerOrgano", new { @idArea = @SessionHelper.GetOrgano(User) }).FirstOrDefault();
            ViewBag.losComponentes = repo.Getdosparam1<Componente>("sp_actividades_obtener_componente_idArea", new { @idArea = @SessionHelper.GetOrgano(User) }).ToList();

            if (elIndicador == 0)
            {
                return View();
            }
            else
            {
                ViewBag.elIdIndica = elIndicador;
                return View(repo.Getdosparam1<AvanceActividad>("sp_avance_actividad_obtener_por_indicador", new { @elIndicador = elIndicador }).ToList());
            }

        }

        [HttpGet]
        public IActionResult EditarAvanceActividadEnlace(int elIndicadorEditar)
        {
            return View(repo.Getdosparam1<AvanceActividad>("sp_avance_actividad_obtner_por_idActividad", new { @idActividad = elIndicadorEditar }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarAvanceActividadEnlace(AvanceActividad avanceActividad)
        {
            avanceActividad.accion = 2;
            repo.Getdosparam1<AvanceActividad>("AgregaActializaAvanceActividad", avanceActividad);
            return RedirectToAction("Index", "AvanceActividadEnlace");
        }
    }
}
