using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.VO
{
    public class VOConta
    {
        [Key]
        public int ID_CONTA { get; set; }

        [Display(Name = "CONTA")]
        public string NM_CONTA { get; set; }

        [Display(Name = "SALDO")]
        public double SALDO_CONTA { get; set; }
    }
}
