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

    public class JustificacionComponenteEnlaceController : Controller
    {
        private DBOperaciones repo;

        public JustificacionComponenteEnlaceController()
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
                return View(repo.Getdosparam1<JustificacionComponenteLista>("sp_justificacion_obtener", new { @opcion = 1, @idJ = elComponente }).ToList());
            }

        }

        public IActionResult EditarJustificacion(int IdJustificacion)
        {
            return RedirectToAction("EditarJustificacion", "JustificacionComponente", new { IdJustificacion = IdJustificacion });
        }
    }
}
