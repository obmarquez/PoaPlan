using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models.Consultas
{
    public class ImpresionAvanceMensual
    {
        public int idComponente { get; set; }
        public string componente { get; set; }
        public string indicador { get; set; }        
        public int totPro { get; set; }
        public int totAva { get; set; }
        public int id { get; set; } //idActividad
        public int enero_ava { get; set; }
        public int febrero_ava { get; set; }
        public int marzo_ava { get; set; }
        public int abril_ava { get; set; }
        public int mayo_ava { get; set; }
        public int junio_ava { get; set; }
        public int julio_ava { get; set; }
        public int agosto_ava { get; set; }
        public int septiembre_ava { get; set; }
        public int octubre_ava { get; set; }
        public int noviembre_ava { get; set; }
        public int diciembre_ava { get; set; }
        public int enero_mod { get; set; }
        public int febrero_mod { get; set; }
        public int marzo_mod { get; set; }
        public int abril_mod { get; set; }
        public int mayo_mod { get; set; }
        public int junio_mod { get; set; }
        public int julio_mod { get; set; }
        public int agosto_mod { get; set; }
        public int septiembre_mod { get; set; }
        public int octubre_mod { get; set; }
        public int noviembre_mod { get; set; }
        public int diciembre_mod { get; set; }
    }
}
