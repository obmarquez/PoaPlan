using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class Estrategia
    {
        public int id { get; set; }
        public int anio { get; set; }

        [Required(ErrorMessage = "Debe indicar el indicador estrategico")]
        [MaxLength(350, ErrorMessage = "Escriba máximo 350 caracteres.")]
        public string indicadorEstrategico { get; set; }
        public int ene_p { get; set; }
        public int feb_p { get; set; }
        public int mar_p { get; set; }
        public int abr_p { get; set; }
        public int may_p { get; set; }
        public int jun_p { get; set; }
        public int jul_p { get; set; }
        public int ago_p { get; set; }
        public int sep_p { get; set; }
        public int oct_p { get; set; }
        public int nov_p { get; set; }
        public int dic_p { get; set; }
        public int ene_a { get; set; }
        public int feb_a { get; set; }
        public int mar_a { get; set; }
        public int abr_a { get; set; }
        public int may_a { get; set; }
        public int jun_a { get; set; }
        public int jul_a { get; set; }
        public int ago_a { get; set; }
        public int sep_a { get; set; }
        public int oct_a { get; set; }
        public int nov_a { get; set; }
        public int dic_a { get; set; }
        public int accion { get; set; }
    }
}
