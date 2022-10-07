using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Models.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private DBOperaciones repo;

        public ConsultasController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult ConcentradoAnual(int anio = 0)
        {
            if(anio == 0)
            {
                return View();
            }
            else
            {
                return View(repo.Getdosparam1<ConcentradoAnual>("sp_concentrado_avance_anual", new { @anio = anio }).ToList());
            }
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult ConcentradoAnualActividad(int anio = 0)
        {
            if (anio == 0)
            {
                return View();
            }
            else
            {
                return View(repo.Getdosparam1<ConcentradoAnual>("sp_concentrado_avance_actividad_anual", new { @anio = anio }).ToList());
            }
        }
    }
}
