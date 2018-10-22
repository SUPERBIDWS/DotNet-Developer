using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Operacoes
{
    class Deposito : IOperacao
    {
        public int Id { get; set; }
        public Conta Requisitante { get; set; }
        public Conta Alvo { get; set; }
        public decimal Valor { get; set; }

        public Deposito()
        {
            var randon = new Random();
            Id = randon.Next(1,1000);
        }
        public void AprovarOperacao()
        {
            Requisitante.Saldo += Valor;
        }
    }
}
