using Entidades;
using Entidades.Operacoes;
using Infraestrutura;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class OperadorTransacao
    {
        private static List<Conta> Contas;
        private Conta contaRequisitante = null;
        private Conta contaAlvo = null;
        public static List<IOperacao> OperacoesPendentes;
        public decimal SaldoOrigem { get { return contaRequisitante.Saldo; } }
        public decimal SaldoDestino { get { return contaAlvo.Saldo; } }

        public OperadorTransacao()
        {
            if (OperacoesPendentes == null)
                OperacoesPendentes = new List<IOperacao>();
            sinconizarDados();
        }

        private void sinconizarDados()
        {
            var dataAccess = new DataAccess();
            if (Contas == null)
                Contas = dataAccess.CarregarContas();
            var sf = new StackTrace().GetFrame(1);
            if (!sf.GetMethod().IsConstructor)// açucar sintático, quero escrever na base apenas quando o sync for chamado por métodos não construtores, 
                                              // Evitando redundância.
                dataAccess.AtualizarContas(Contas);
        }

        /// <summary>
        /// Aprova a operação e remove da lista de operações pendentes
        /// </summary>
        /// <param name="operacao"></param>
        public void AprovarOperacao(IOperacao operacao)
        {
            operacao.AprovarOperacao();
            var op = OperacoesPendentes.Find(x => x.Id == operacao.Id);
            OperacoesPendentes.Remove(op);
            sinconizarDados();
        }

        public IOperacao ObterOperacao(int id)
        {
            return OperacoesPendentes.Find(x => x.Id == id);
        }

        /// <summary>
        /// Cria uma nova operação e a armazena no pool de operações para aprovação
        /// retorna 1 se a operação foi criada, -1 se a operação não foi criada
        /// </summary>
        /// <param name="TipoOperacao">Tipo de operação desejada</param>
        /// <param name="IdRequisitante">Id da conta que solicita a operação</param>
        /// <param name="IdAlvo">Quando aplicável, id da conta que recebe a operação</param>
        public IOperacao NovaOperacao(TipoOperacao TipoOperacao)
        {
            try
            {
                var operacao = OperacaoFactory.ObterOperacao(TipoOperacao, contaRequisitante, contaAlvo);
                OperacoesPendentes.Add(operacao);
                return operacao;
            }
            catch
            {
                return null;
            }


        }

        /// <summary>
        /// Seleciona as contas envolvidas na operação
        /// </summary>
        /// <param name="tipoConta">Conta de origem ou de destino</param>
        /// <param name="NomeConta">Nome da conta para a busca</param>
        public int SelecionarConta(TipoConta tipoConta, string NomeConta)
        {
            switch (tipoConta)
            {
                case TipoConta.Origem:
                    {
                        contaRequisitante = Contas.Find(x => x.Nome.Equals(NomeConta, StringComparison.InvariantCultureIgnoreCase));
                        if (contaRequisitante != null)
                            return 1;
                        return 0;

                    }
                case TipoConta.Destino:
                    {
                        contaAlvo = Contas.Find(x => x.Nome.Equals(NomeConta, StringComparison.InvariantCultureIgnoreCase));
                        if (contaAlvo != null)
                            return 1;
                        return 0;
                    }
            }
            return -1;
        }

    }
}
