using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class JustificacionComponenteController : Controller
    {
        private DBOperaciones repo;

        public JustificacionComponenteController()
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
                return View(repo.Getdosparam1<JustificacionComponenteLista>("sp_justificacion_obtener", new { @opcion = 1, @idJ = elComponente }).ToList());
            }
        }

        [HttpGet]
        public IActionResult NuevaJustificacion(int elIdComp)
        {
            ViewBag.IdComp = elIdComp;

            //ViewBag.losMeses = Meses();
            ViewBag.losMeses = Constantes.Meses();

            return View();
        }

        [HttpPost]
        public IActionResult NuevaJustificacion(JustificacionComponente laJustificacion)
        {
            laJustificacion.tipoJustificacion = 1;
            laJustificacion.accion = 1;
            repo.Getdosparam1<JustificacionComponente>("AgregaActualizaJustificacion", laJustificacion);
            if(@SessionHelper.GetNameRol(User) == "Enlace")
            {
                return RedirectToAction("Index", "JustificacionComponenteEnlace");
            }
            else
            {
                return RedirectToAction("Index", "JustificacionComponente");
            }            
        }

        [HttpGet]
        public IActionResult EditarJustificacion(int IdJustificacion)
        {
            //Para combo de meses
            //ViewBag.losMeses = Meses();
            ViewBag.losMeses = Constantes.Meses();

            return View(repo.Getdosparam1<JustificacionComponente>("sp_justificacion_obtener", new { @opcion = 2, @idJ = IdJustificacion }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarJustificacion(JustificacionComponente laJustificacion)
        {
            laJustificacion.tipoJustificacion = 1;
            laJustificacion.accion = 2;
            repo.Getdosparam1<JustificacionComponente>("AgregaActualizaJustificacion", laJustificacion);
            if (@SessionHelper.GetNameRol(User) == "Enlace")
            {
                return RedirectToAction("Index", "JustificacionComponenteEnlace");
            }
            else
            {
                return RedirectToAction("Index", "JustificacionComponente");
            }
        }

    }
}
