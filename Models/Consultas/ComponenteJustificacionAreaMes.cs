using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models.Consultas
{
    public class ComponenteJustificacionAreaMes
    {
        public int idArea { get; set; }
        public int idComponente { get; set; }
        public string componente { get; set; }
        public string analisis { get; set; }
        public string mes { get; set; }
    }
}
