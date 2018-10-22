using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using Entidades;


namespace Main
{
    class Program
    {
        static int Main(string[] args)
        {
            OperadorTransacao core = new OperadorTransacao();
            Console.WriteLine("*** Teste S4Pay ***");
            Console.WriteLine("Autor:Evaristo Filho");

            Console.WriteLine("Os comando e argumentos são separados por espaços");
            Console.WriteLine("Digite o comando 'SelecionarConta' com os argumentos 'O' para origem ou 'D' para destino seguido do nome da conta");
            Console.WriteLine("Digite o comando 'CriarOperacao' com os argumentos 'S' para Saque 'D' para depósito ou 'T' para transferencia");
            Console.WriteLine("Digite o comando 'AprovarOperacao' com id da operação para aprova-la");
            Console.WriteLine("Digite o comando 'ListarOperacoes' Para ver as operações pendentes aprovação.");
            Console.WriteLine("Digite o comando 'Saldo' com os argumentos 'O' para conta de origem ou 'D' para conta de destino ");
            Console.WriteLine("Digite o comando 'Sair' para sair");

            while (true)
            {
                var comando = Console.ReadLine().ToLower();// forçar case insensitive
                var comandoCompleto = comando.Split(' ').ToList();
                comando = comandoCompleto[0];
                switch (comando)
                {
                    case "sair": { return 1; }
                    case "cls": { Console.Clear(); init(); break; }
                    case "help": { init(); break; }

                    case "selecionarconta":
                        {
                            if (comandoCompleto[1].Equals("d"))
                                if (core.SelecionarConta(TipoConta.Destino, comandoCompleto[2]) == 1)
                                {
                                    Console.WriteLine("Conta de Destino selecionada");
                                    Console.WriteLine("Saldo disponível:{0}", core.SaldoDestino);
                                }
                                else
                                    Console.WriteLine("Nâo foi possível selecionar a conta");

                            if (comandoCompleto[1].Equals("o"))
                                if (core.SelecionarConta(TipoConta.Origem, comandoCompleto[2]) == 1)
                                {
                                    Console.WriteLine("Conta de Origem selecionada");
                                    Console.WriteLine("Saldo disponível:{0}", core.SaldoOrigem);
                                }
                                else
                                    Console.WriteLine("Nâo foi possível selecionar a conta");
                            break;
                        }
                    case "criaroperacao":
                        {
                            switch (comandoCompleto[1])
                            {
                                case "d":
                                    {
                                        var operacao = core.NovaOperacao(Entidades.Operacoes.TipoOperacao.Deposito);
                                        if (operacao != null)
                                        {
                                            PrepararOperacao(operacao);
                                        }
                                        else { Console.WriteLine("Ocorreu um erro na criação da operação"); }
                                        break;
                                    }
                                case "t":
                                    {
                                        var operacao = core.NovaOperacao(Entidades.Operacoes.TipoOperacao.Transferencia);
                                        if (operacao != null)
                                        {
                                            PrepararOperacao(operacao);
                                        }
                                        else { Console.WriteLine("Ocorreu um erro na criação da operação"); }
                                        break;
                                    }
                                case "s":
                                    {
                                        var operacao = core.NovaOperacao(Entidades.Operacoes.TipoOperacao.Saque);
                                        if (operacao != null)
                                        {
                                            PrepararOperacao(operacao);
                                        }
                                        else { Console.WriteLine("Ocorreu um erro na criação da operação"); }
                                        break;
                                    }

                            }
                            break;
                        }
                    case "aprovaroperacao":
                        {
                            int id;
                            if (int.TryParse(comandoCompleto[1], out id))
                            {
                                var operacao = core.ObterOperacao(id);
                                core.AprovarOperacao(operacao);
                                Console.WriteLine("Operação Aprovada");
                            }
                            else
                                Console.WriteLine("Id de operação inválido");

                            break;
                        }
                    case "listaroperacoes":
                        {
                            foreach (var operacao in OperadorTransacao.OperacoesPendentes)
                            {
                                Console.WriteLine("Id da operação:{0}, Valor da operação:{1}, Tipo Da operação:{2}", operacao.Id, operacao.Valor, operacao.GetType().Name);
                            }
                            break;
                        }
                    case "saldo": { init(); break; }

                    default: Console.WriteLine("Comando Inválido"); break;

                }
            }






            return 0;
        }

        private static void PrepararOperacao(IOperacao operacao)
        {
            Console.WriteLine("Digite o valor:");
            decimal valor;
            if (decimal.TryParse(Console.ReadLine(), out valor))
            {
                operacao.Valor = valor;
                Console.WriteLine("Operação agendada com sucesso. Id da operação:{0}", operacao.Id);
            }
            else
            {
                Console.WriteLine("Ocorreu um erro na preparação da operação");

            }
        }

        private static void init()
        {
            Console.WriteLine("Os comando e argumentos são separados por espaços");
            Console.WriteLine("Digite o comando 'SelecionarConta' com os argumentos 'O' para origem ou 'D' para destino");
            Console.WriteLine("Digite o comando 'CriarOperacao' com os argumentos 'S' para Saque 'D' para depósito ou 'T' para transferencia");
            Console.WriteLine("Digite o comando 'AprovarOperacao' com id da operação para aprova-la");
            Console.WriteLine("Digite o comando 'ListarOperacoes' Para ver as operações pendentes aprovação.");
            Console.WriteLine("Digite o comando 'Sair' para sair");
        }
    }
}
