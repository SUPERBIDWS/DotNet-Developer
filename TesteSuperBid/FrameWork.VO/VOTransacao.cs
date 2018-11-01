using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FrameWork.VO
{
    public class VOTransacao
    {
        [Key]
        [JsonProperty]
        public int ID_TRANSACAO { get; set; }

        [Required(ErrorMessage = "Favor Selecionar uma Conta de Origem")]
        [Display(Name = "CONTA ORIGEM")]
        [JsonProperty]
        public int IDE_CONTA_ORIGEM { get; set; }

        [Required(ErrorMessage = "Favor Selecionar uma Conta de Destino")]
        [Display(Name = "CONTA DESTINO")]
        [JsonProperty]
        public int IDE_CONTA_DESTINO { get; set; }

        [Required(ErrorMessage = "Favor Informar um Valor")]
        [Display(Name = "VALOR")]
        [JsonProperty]
        public double VALOR { get; set; }

        [Display(Name = "STATUS")]
        [JsonProperty]
        public int IDE_STATUS { get; set; }

        [Display(Name = "DATA DA TRANSAÇÃO")]
        [JsonProperty]
        public DateTime? DATA_TRANSACAO { get; set; }

        [Display(Name = "DATA DO RESULTADO")]
        [JsonProperty]
        public DateTime? DATA_RESULTADO { get; set; }

        public virtual VOConta DESTINO { get; set; }
        public virtual VOConta ORIGEM { get; set; }
        public virtual VOStatus STATUS { get; set; }
    }
}
