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

    public class JustificacionActividadEnlaceController : Controller
    {
        private DBOperaciones repo;

        public JustificacionActividadEnlaceController()
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
                return View(repo.Getdosparam1<JustificacionActividadLista>("sp_justificacion_obtener", new { @opcion = 3, @idJ = elIndicador }).ToList());
            }

        }

        public IActionResult EditarJustificacionActividad(int idJustificacion)
        {
            return RedirectToAction("EditarJustificacionActividad", "JustificacionActividad", new { idJustificacion = idJustificacion });
        }

        [HttpGet]
        public IActionResult AdjuntarPdf(string mes)
        {
            ViewBag.mesPdf = mes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdjuntarPdf([FromForm] IFormFile archivo_pdf, string elMes)
        {
            //para el Servidor
            //string fullNewName = elMes + "_" + @SessionHelper.GetOrgano(User) + "_" + archivo_pdf.FileName;
            string fullNewName = archivo_pdf.FileName;

            //para el proeycto
            //var FullPathFile = "C:/Pdfs/" + fullNewName;
            var FullPathFile = "C:/inetpub/wwwroot/PoaPlan/wwwroot/files/" + fullNewName;

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
