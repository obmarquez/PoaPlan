using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class Componente
    {
        public int idComponente { get; set; }
        public int idArea { get; set; }

        [Required(ErrorMessage = "Debe indicar la descripción del componente")]
        public string componente { get; set; }

        [Required(ErrorMessage = "Debe indicar la descripción del indicador")]
        public string indicador { get; set; }
        public int accion { get; set; }
    }
}
