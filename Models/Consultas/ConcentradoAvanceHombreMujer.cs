using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models.Consultas
{
    public class ConcentradoAvanceHombreMujer
    {
        public string organo { get; set; }
        public string componente { get; set; }
        public string indicador { get; set; }
        public int idComponente { get; set; }
        public int totPro { get; set; }
        public int totAva { get; set; }
        public int totHom { get; set; }
        public int totMuj { get; set; }
    }
}
