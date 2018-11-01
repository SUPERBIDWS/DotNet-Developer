using FrameWork.VO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAprovacao
{
    class Program
    {
        static private List<VOTransacao> listaTransacoes;

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.WriteLine(" ");
                Console.WriteLine(" Iniciando programa Aprovação de Transações...");
                Console.WriteLine(" Escolha uma das opções:");
                Console.WriteLine(" [ 1 ] Buscar Transações Pendentes");

                if (listaTransacoes != null && listaTransacoes.Count > 0)
                {
                    Console.WriteLine(" [ 2 ] Verificar as " + listaTransacoes.Count + " Transações Pendentes.");
                }

                Console.WriteLine(" [ 9 ] Sair");
                Console.WriteLine("-------------------------------------");

                int.TryParse(Console.ReadLine(), out opcao);

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine(" Buscando Transações...");
                        BuscarTransacoes();
                        TratarRetorno();

                        break;

                    case 2:
                        GerenciarTransacoes();
                        BuscarTransacoes();

                        break;

                    case 9:
                        Console.WriteLine(" Finalizando...");
                        break;

                    default:
                        Console.WriteLine(" Favor Escolher uma das Opções.");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
            while (opcao != 9);
        }

        static void BuscarTransacoes()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/GetTransacoesPendentes");
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    listaTransacoes = JsonConvert.DeserializeObject<List<VOTransacao>>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Um erro ocorreu ao buscar as Transações Pendentes.");
            }
        }

        static void AtualizarTransacao(VOTransacao transacao, int status)
        {
            try
            {
                transacao.IDE_STATUS = status;

                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/PostAlterarTransacao/");
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(transacao);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    transacao = JsonConvert.DeserializeObject<VOTransacao>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Um erro ocorreu ao atualizar a Transação.");
            }
        }

        static void TratarRetorno()
        {
            if (listaTransacoes != null)
            {
                if (listaTransacoes.Count > 0)
                {
                    Console.WriteLine(" Foram encontradas " + listaTransacoes.Count + " Transações Pendentes.");
                }
                else
                {
                    Console.WriteLine(" Não foram encontradas Transações Pendentes.");
                }
            }
            else
            {
                Console.WriteLine(" Um erro ocorreu ao buscar as Transações Pendentes.");
            }
        }

        static void GerenciarTransacoes()
        {
            string entrada;
            bool processada;

            foreach (var item in listaTransacoes)
            {
                entrada = string.Empty;
                processada = false;

                Console.WriteLine(" Transação ID " + item.ID_TRANSACAO);
                Console.WriteLine(" Origem: " + item.ORIGEM.NM_CONTA);
                Console.WriteLine(" Destino: " + item.DESTINO.NM_CONTA);
                Console.WriteLine(" Valor: " + item.VALOR);
                Console.WriteLine(" Data: " + item.DATA_TRANSACAO.Value.ToString("dd/MM/yyyy HH:mm"));
                Console.WriteLine(" Aprovar Transação? S/N");

                do
                {
                    entrada = Console.ReadLine();

                    switch (entrada)
                    {
                        case "S":
                        case "s":
                            Console.WriteLine(" Processando...");
                            AtualizarTransacao(item, 2);
                            Console.WriteLine(" Realizado com sucesso.");

                            processada = true;
                            break;

                        case "N":
                        case "n":
                            Console.WriteLine(" Processando...");
                            AtualizarTransacao(item, 3);
                            Console.WriteLine(" Realizado com sucesso.");

                            processada = true;
                            break;

                        default:
                            Console.WriteLine(" Favor Escolher uma das Opções.");
                            break;
                    }
                }
                while (!processada);

                Console.WriteLine("-------------------------------------");

                entrada = string.Empty;
                processada = false;

                bool finalizar = false;

                if (item != listaTransacoes.Last())
                {
                    Console.WriteLine(" Continuar para a próxima Transação? S/N");

                    do
                    {
                        entrada = Console.ReadLine();

                        switch (entrada)
                        {
                            case "S":
                            case "s":
                                processada = true;
                                Console.WriteLine(" Buscando a próxima...");
                                break;

                            case "N":
                            case "n":
                                processada = true;
                                finalizar = true;
                                Console.WriteLine(" Retornando as opções...");
                                break;

                            default:
                                Console.WriteLine(" Favor Escolher uma das Opções.");
                                break;
                        }
                    }
                    while (!processada);

                    if (finalizar) break;
                }
                else
                {
                    Console.WriteLine(" Transações Finalizadas.");
                }
            }
        }
    }
}
