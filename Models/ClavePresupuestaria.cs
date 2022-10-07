using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class ClavePresupuestaria
    {
        public int idarea { get; set; }

        [Required(ErrorMessage ="Debe indicar una nueva clave")]
        public string clave { get; set; }

        [Required(ErrorMessage ="Debe indicar el organo administrativo de la clave")]
        public string organo { get; set; }

        [Required(ErrorMessage = "Debe indicar la descripcion del proyecto")]
        public string proyecto { get; set; }

        [Required(ErrorMessage ="Debe indicar el año operativo")]
        public int anio { get; set; }
        public bool activo { get; set; }
        public int accion { get; set; }
        public string lider { get; set; }
    }
}
