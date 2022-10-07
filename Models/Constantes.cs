using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class Constantes
    {
        public static List<SelectListItem> Meses()
        {
            List<SelectListItem> meses = new List<SelectListItem>();
            meses.Add(new SelectListItem { Text = "Enero", Value = "Enero" });
            meses.Add(new SelectListItem { Text = "Febrero", Value = "Febrero" });
            meses.Add(new SelectListItem { Text = "Marzo", Value = "Marzo" });
            meses.Add(new SelectListItem { Text = "Abril", Value = "Abril" });
            meses.Add(new SelectListItem { Text = "Mayo", Value = "Mayo" });
            meses.Add(new SelectListItem { Text = "Junio", Value = "Junio" });
            meses.Add(new SelectListItem { Text = "Julio", Value = "Julio" });
            meses.Add(new SelectListItem { Text = "Agosto", Value = "Agosto" });
            meses.Add(new SelectListItem { Text = "Septiembre", Value = "Septiembre" });
            meses.Add(new SelectListItem { Text = "Octubre", Value = "Octubre" });
            meses.Add(new SelectListItem { Text = "Noviembre", Value = "Noviembre" });
            meses.Add(new SelectListItem { Text = "Diciembre", Value = "Diciembre" });
            return meses;
        }
    }
}
