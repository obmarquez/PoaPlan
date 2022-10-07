using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoaPlan.Models
{
    public class avanceComponente
    {
        public int id { get; set; }
        public int idComponente { get; set; }

        public int @enero_ap { get; set; }
        public int @febrero_ap { get; set; }
        public int @marzo_ap { get; set; }
        public int @abril_ap { get; set; }
        public int @mayo_ap { get; set; }
        public int @junio_ap { get; set; }
        public int @julio_ap { get; set; }
        public int @agosto_ap { get; set; }
        public int @septiembre_ap { get; set; }
        public int @octubre_ap { get; set; }
        public int @noviembre_ap { get; set; }
        public int @diciembre_ap { get; set; }
        //---------------------------------------------Programado
        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int enero_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int febrero_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int marzo_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int abril_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int mayo_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int junio_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int julio_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int agosto_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int septiembre_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int octubre_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int noviembre_mod { get; set; }

        [Required(ErrorMessage = "Debe indicar una cantidad")]
        public int diciembre_mod { get; set; }

        //--------------------------------------------- Avance
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

        public int hene { get; set; }
        public int hfeb { get; set; }
        public int hmar { get; set; }
        public int habr { get; set; }
        public int hmay { get; set; }
        public int hjun { get; set; }
        public int hjul { get; set; }
        public int hago { get; set; }
        public int hsep { get; set; }
        public int hoct { get; set; }
        public int hnov { get; set; }
        public int hdic { get; set; }

        public int mene { get; set; }
        public int mfeb { get; set; }
        public int mmar { get; set; }
        public int mabr { get; set; }
        public int mmay { get; set; }
        public int mjun { get; set; }
        public int mjul { get; set; }
        public int mago { get; set; }
        public int msep { get; set; }
        public int moct { get; set; }
        public int mnov { get; set; }
        public int mdic { get; set; }

        public int accion { get; set; }
    }
}
