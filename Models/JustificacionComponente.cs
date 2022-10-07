using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class JustificacionComponente
    {
        public int idJustificacion { get; set; }
        public int idIndicador { get; set; }
        public string mes { get; set; }

        [Required(ErrorMessage = "Debe indicar el analsis")]
        [MaxLength(1000, ErrorMessage = "Escriba máximo 1,000 caracteres.")]
        public string analisis { get; set; }

        [Required(ErrorMessage = "Debe indicar la justificación")]
        [MaxLength(1000, ErrorMessage = "Escriba máximo 1,000 caracteres.")]
        public string justificacion { get; set; }

        public int tipoJustificacion { get; set; }
        public int accion { get; set; }
    }
}
