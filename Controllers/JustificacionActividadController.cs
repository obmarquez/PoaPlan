using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Helper;
using PoaPlan.Models;
using PoaPlan.Models.Consultas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{
    [Authorize]

    public class JustificacionActividadController : Controller
    {
        private DBOperaciones repo;

        public JustificacionActividadController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index(int elIndicador = 0)
        {
            ViewBag.ComboAnioArea = repo.Getdosparam1<ClavePresupuestaria>("sp_actividades_lista_anio_organo", null).ToList();

            if (elIndicador == 0)
            {
                return View();
            }
            else
            {
                ViewBag.elIdIndica = elIndicador;
                return View(repo.Getdosparam1<JustificacionActividadLista>("sp_justificacion_obtener", new { @opcion = 3, @idJ = elIndicador }).ToList());
            }
        }

        [HttpGet]
        public IActionResult NuevaJustificacionActividad(int elIdAvAc)
        {
            ViewBag.elIndicador = elIdAvAc;

            ViewBag.losMeses = Constantes.Meses();

            return View();
        }

        [HttpPost]
        public IActionResult NuevaJustificacionActividad(JustificacionComponente laJustificacion)
        {
            laJustificacion.tipoJustificacion = 2;
            laJustificacion.accion = 1;
            repo.Getdosparam1<JustificacionComponente>("AgregaActualizaJustificacion", laJustificacion);

            if (@SessionHelper.GetNameRol(User) == "Enlace")
            {
                return RedirectToAction("Index", "JustificacionActividadEnlace");
            }
            else
            {
                return RedirectToAction("Index", "JustificacionActividad");
            }
        }

        [HttpGet]
        public IActionResult EditarJustificacionActividad(int idJustificacion)
        {
            ViewBag.losMeses = Constantes.Meses();

            return View(repo.Getdosparam1<JustificacionComponente>("sp_justificacion_obtener", new { @opcion = 4, @idJ = idJustificacion }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditarJustificacionActividad(JustificacionComponente laJustificacion)
        {
            laJustificacion.tipoJustificacion = 2;
            laJustificacion.accion = 2;
            repo.Getdosparam1<JustificacionComponente>("AgregaActualizaJustificacion", laJustificacion);
            if (@SessionHelper.GetNameRol(User) == "Enlace")
            {
                return RedirectToAction("Index", "JustificacionActividadEnlace");
            }
            else
            {
                return RedirectToAction("Index", "JustificacionActividad");
            }
        }

        [HttpGet]
        public IActionResult AdjuntarPdf(string mes)
        {
            ViewBag.mesPdf = mes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdjuntarPdf([FromForm]IFormFile archivo_pdf, string elMes)
        {
            string fullNewName = elMes + "_" + @SessionHelper.GetOrgano(User) + "_" + archivo_pdf.FileName;
            //var FullPathFile = "C:/Pdfs/" + fullNewName;
            var FullPathFile = "C:/Net 2012/PoaPlan/wwwroot/files/" + fullNewName;

            using (FileStream fs = System.IO.File.Create(FullPathFile))
            {
                await archivo_pdf.CopyToAsync(fs);
                await fs.FlushAsync();
            }
            //return fullNewName;

            if (@SessionHelper.GetNameRol(User) == "Enlace")
            {
                return RedirectToAction("Index", "JustificacionActividadEnlace");
            }
            else
            {
                return RedirectToAction("Index", "JustificacionActividad");
            }
        }

    }
}
