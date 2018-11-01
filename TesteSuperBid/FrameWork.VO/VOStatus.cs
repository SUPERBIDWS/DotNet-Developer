using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.VO
{
    public class VOStatus
    {
        [Key]
        public int ID_STATUS { get; set; }

        [Display(Name = "STATUS")]
        public string NM_STATUS { get; set; }
    }
}
