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
    public class AvanceComponenteController : Controller
    {
        private DBOperaciones repo;

        public AvanceComponenteController()
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
                return View(repo.Getdosparam1<avanceComponente>("sp_avance_componente_obtener", new { @idCom = elComponente }).ToList());
            }
        }

        [HttpGet]
        public IActionResult NuevoAvanceComponente(int elIdComp)
        {
            ViewBag.elIdComp_a = elIdComp;
            ViewBag.organo_componente = repo.Getdosparam1<OrganoComponente>("sp_organo_componente_obtener", new { @idC = elIdComp }).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public IActionResult NuevoAvanceComponente(avanceComponente miavanceComponente)
        {
            miavanceComponente.enero_ap = 0;
            miavanceComponente.febrero_ap = 0;
            miavanceComponente.marzo_ap = 0;
            miavanceComponente.abril_ap = 0;
            miavanceComponente.mayo_ap = 0;
            miavanceComponente.junio_ap = 0;
            miavanceComponente.julio_ap = 0;
            miavanceComponente.agosto_ap = 0;
            miavanceComponente.septiembre_ap = 0;
            miavanceComponente.octubre_ap = 0;
            miavanceComponente.noviembre_ap = 0;
            miavanceComponente.diciembre_ap = 0;
            miavanceComponente.accion = 1;
            repo.Getdosparam1<avanceComponente>("AgregaActualizaAvanceComponentes", miavanceComponente);
            return RedirectToAction("Index", "AvanceComponente");
        }

        [HttpGet]
        public IActionResult EditarAvanceComponente(int elIdCompAvaEd, int elIdComp)
        {
            ViewBag.organo_componente = repo.Getdosparam1<OrganoComponente>("sp_organo_componente_obtener", new { @idC = elIdComp }).FirstOrDefault();
            return View(repo.Getdosparam1<avanceComponente>("sp_avance_componente_obtener_por_id", new { @idAC = elIdCompAvaEd}).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarAvanceComponente(avanceComponente edavanceComponente)
        {
            edavanceComponente.enero_ap = 0;
            edavanceComponente.febrero_ap = 0;
            edavanceComponente.marzo_ap = 0;
            edavanceComponente.abril_ap = 0;
            edavanceComponente.mayo_ap = 0;
            edavanceComponente.junio_ap = 0;
            edavanceComponente.julio_ap = 0;
            edavanceComponente.agosto_ap = 0;
            edavanceComponente.septiembre_ap = 0;
            edavanceComponente.octubre_ap = 0;
            edavanceComponente.noviembre_ap = 0;
            edavanceComponente.diciembre_ap = 0;
            edavanceComponente.accion = 2;
            repo.Getdosparam1<avanceComponente>("AgregaActualizaAvanceComponentes", edavanceComponente);
            return RedirectToAction("Index", "AvanceComponente");
        }
    }
}
