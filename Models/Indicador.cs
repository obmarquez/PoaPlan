using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class Indicador
    {
        public int idIndicador { get; set; }
        public int idcomponente { get; set; }

        [Required(ErrorMessage = "Debe indicar la descripción del objetivo")]
        [MaxLength(200, ErrorMessage = "Escriba máximo 7 caracteres.")]
        public string objetivo { get; set; }

        [Required(ErrorMessage = "Debe indicar la descripción del indicador")]
        [MaxLength(200, ErrorMessage = "Escriba máximo 7 caracteres.")]
        public string indicador { get; set; }

        public int accion { get; set; }
    }
}
