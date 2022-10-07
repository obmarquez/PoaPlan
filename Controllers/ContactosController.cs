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

    public class ContactosController : Controller
    {
        private DBOperaciones repo;

        public ContactosController()
        {
            repo = new DBOperaciones();
        }

        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Index()
        {
            return View(repo.Getdosparam1<Enlaces>("sp_enlaces_obtener_datos", null).ToList());
        }
    }
}
