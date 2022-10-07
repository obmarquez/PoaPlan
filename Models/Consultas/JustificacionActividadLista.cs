using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models.Consultas
{
    public class JustificacionActividadLista
    {
        public int idJustificacion { get; set; }
        public int idindicador { get; set; }
        public string mes { get; set; }
        public string analisis_act { get; set; }
        public string justificacion_act { get; set; }
    }
}
