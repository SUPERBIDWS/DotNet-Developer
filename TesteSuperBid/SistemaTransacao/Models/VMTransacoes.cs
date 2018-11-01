using FrameWork.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaTransacao.Models
{
    public class VMTransacoes
    {
        public VOConta ContaOrigem { get; set; }

        public VOConta ContaDestino { get; set; }

        public List<VOConta> Contas { get; set; }

        public VOTransacao Transacao { get; set; }

        public List<VOTransacao> Transacoes { get; set; }

        public string MensagemRetorno { get; set; }

        public bool boolRetorno { get; set; }

        public string VALOR { get; set; }
    }
}