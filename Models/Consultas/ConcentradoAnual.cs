using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models.Consultas
{
    public class ConcentradoAnual
    {
        public int idarea { get; set; }
        public string organo { get; set; }
        public string componente { get; set; }
        public string programado { get; set; }
        public string avance { get; set; }
        public string porcentaje { get; set; }
    }
}
