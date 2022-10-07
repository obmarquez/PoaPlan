using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Helper;
using PoaPlan.Models;
using PoaPlan.Models.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{
    [Authorize]

    public class AvanceComponenteEnlaceController : Controller
    {
        private DBOperaciones repo;

        public AvanceComponenteEnlaceController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Enlace")]
        public IActionResult Index(int elComponente = 0)
        {
            ViewBag.elOrgano = repo.Getdosparam1<ClavePresupuestaria>("obtenerOrgano", new { @idArea = @SessionHelper.GetOrgano(User) }).FirstOrDefault();
            ViewBag.losComponentes = repo.Getdosparam1<Componente>("sp_actividades_obtener_componente_idArea", new { @idArea = @SessionHelper.GetOrgano(User) }).ToList();

            if (elComponente == 0)
            {                                

                return View();
            }
            else
            {
                ViewBag.idC = elComponente;
                return View(repo.Getdosparam1<avanceComponente>("sp_avance_componente_obtener", new { @idCom = elComponente }).ToList());
            }            

        }

        public IActionResult EditarAvanceComponenteEnlace(int elIdCompAvaEd, int elIdComp)
        {
            ViewBag.organo_componente = repo.Getdosparam1<OrganoComponente>("sp_organo_componente_obtener", new { @idC = elIdComp }).FirstOrDefault();
            return View(repo.Getdosparam1<avanceComponente>("sp_avance_componente_obtener_por_id", new { @idAC = elIdCompAvaEd }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarAvanceComponenteEnlace(avanceComponente edavanceComponente)
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
            return RedirectToAction("Index", "AvanceComponenteEnlace");
        }
    }
}
