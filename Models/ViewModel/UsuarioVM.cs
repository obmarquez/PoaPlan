using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PoaPlan.Models.ViewModel
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "Escriba su usuario.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Escriba su contraseña.")]
        public string Clave { get; set; }
    }
}
