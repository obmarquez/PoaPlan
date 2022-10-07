using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PoaPlan.Data;
using PoaPlan.Helper;
using PoaPlan.Models;
using PoaPlan.Models.Consultas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private DBOperaciones repo;
        CodeStackCTX ctx;

        public HomeController(CodeStackCTX _ctx)
        {
            repo = new DBOperaciones();
            ctx = _ctx;
        }

        public IActionResult Index()
        {
            ViewBag.ind_est_evolucion = repo.Getdosparam1<PoaPlan.Models.Consultas.Ind_Est_Evolucion>("sp_indicador_estrategico_evolucion", null).ToList();
            ViewBag.datosEnlaceConcentrado = repo.Getdosparam1<PoaPlan.Models.Consultas.ConcentradoAvanceHombreMujer>("sp_concentrado_componente_area", new { @idArea = @SessionHelper.GetOrgano(User) }).ToList();
            ViewBag.datosTotalIndicadorEstrateico = repo.Getdosparam1<PoaPlan.Models.Consultas.IndiceEstrategicoTotales>("sp_indicadorestrategico_totales", null).FirstOrDefault();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Registro()
        {
            ViewBag.losOrganos = repo.Getdosparam1<ListaOrganos>("sp_organos_obtener", null).ToList();

            return View();
        }

        [BindProperty]
        public Usuarios Usuario { get; set; }
        public async Task<IActionResult> Registrar()
        {
            var result = await ctx.Usuarios.Where(x => x.Nombre == Usuario.Nombre).SingleOrDefaultAsync();
            if (result != null)
            {
                return BadRequest(new JObject() {
                    { "Statuscode",  400 },
                    { "Message", "El usuario ya existe seleccione otro."  }
                });
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                }
                else
                {
                    var hash = HashHelper.Hash(Usuario.Clave);
                    Usuario.Clave = hash.Password;
                    Usuario.Sal = hash.Salt;
                    Usuario.Activo = true;
                    ctx.Usuarios.Add(Usuario);
                    await ctx.SaveChangesAsync();
                    Usuario.Clave = "";
                    Usuario.Sal = "";
                    return Created($"/Usuarios/{Usuario.IdUsuario}", Usuario);
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
