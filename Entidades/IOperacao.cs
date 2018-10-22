using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public interface IOperacao
    {
        int Id { get; }
        Conta Requisitante { get; set; }
        Conta Alvo { get; set; }
        decimal Valor {get;set; }
        void AprovarOperacao();

    }

}
