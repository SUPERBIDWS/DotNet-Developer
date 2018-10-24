using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    class Saque : IOperacao
    {
        public Conta Requisitante { get; set; }
        public Conta Alvo { get; set; }
        public int Id { get; }
        public decimal Valor { get; set; }

        public Saque()
        {
            var randon = new Random();
            Id = randon.Next(1, 1000);
        }
        public void AprovarOperacao()
        {
            if (this.Valor <= Requisitante.Saldo)
                Requisitante.Saldo -= Valor;
            else
                throw new InvalidOperationException("Saldo Insuficiente");
        }
    }
}
