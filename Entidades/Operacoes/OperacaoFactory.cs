using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Operacoes
{
    /// <summary>
    /// Factory responsável por criar uma nova operação
    /// </summary>
    public static class OperacaoFactory
    {
        /// <summary>
        /// Retorna uma operação sob a interface IOperação para ser consumida pelo nucleo da aplicação.
        /// </summary>
        /// <param name="TipoOperacao"></param>
        /// <param name="Requisitante">Objeto Representando a origem da operação</param>
        /// <param name="Alvo">Objeto representando o destino da operação quando aplicável</param>
        /// <returns></returns>
        public static IOperacao ObterOperacao(TipoOperacao TipoOperacao, Conta Requisitante, Conta Alvo = null)
        {
            switch (TipoOperacao)
            {

                case TipoOperacao.Deposito:
                    return new Deposito { Alvo = Alvo, Requisitante = Requisitante };

                case TipoOperacao.Saque:

                    return new Saque { Alvo = Alvo, Requisitante = Requisitante };

                case TipoOperacao.Transferencia:
                    if (Alvo == null)
                        throw new InvalidOperationException("Não é possível fazer uma transferencia sem destino");
                    return new Transferencia { Alvo = Alvo, Requisitante = Requisitante };

            }
            return null;
        }
    }
}
