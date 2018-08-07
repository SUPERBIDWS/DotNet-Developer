using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Superbid.ConsoleApp.AppModel;
using Superbid.Domain.DomainModels;


namespace Superbid.ConsoleApp
{
    class Program
    {
        static ApplicationService applicationService;
        static void Main(string[] args)
        {
            Console.WriteLine("Digite a url da api");
            string url = Console.ReadLine();
            applicationService = new ApplicationService(url);
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("Digite:");
                Console.WriteLine("1 para criar uma conta");
                Console.WriteLine("2 para listar contas");
                Console.WriteLine("3 para transfêrencia");
                Console.WriteLine("4 para listar transferências pendentes");
                Console.WriteLine("5 para confirmar transferência");
                Console.WriteLine("ou sair");

                string command = Console.ReadLine();
                if (command.ToLower() == "sair")
                {
                    return;
                }

                int intCommand;
                if (int.TryParse(command, out intCommand))
                {
                    await DecodeCommand(intCommand);
                }
            }
        }

        private static async Task DecodeCommand(int command)
        {
            switch (command)
            {
                //Create account
                case 1:
                {
                    Console.Clear();
                    AccountModel account = await applicationService.CreateAccount();
                    Console.Write("Conta criada id - {0}\t Saldo Total - {1}", account.Id, account.TotalBalance);
                    break;
                }
                case 2:
                {
                    Console.Clear();
                    IList<AccountModel> accountList = await applicationService.GetAccounts();
                    Console.WriteLine(new string('=', 100));
                    Console.WriteLine("Id                      \tSaldo");
                    Console.WriteLine(new string('=', 100));

                    foreach (var account in accountList)
                    {
                        Console.WriteLine("{0}\t{1}", account.Id, account.TotalBalance);
                    }
                    Console.WriteLine(new string('=', 100));

                    break;
                }
                case 3:
                {
                    await applicationService.TransferAmmount();
                    break;
                }
                case 4:
                {
                    IList<TransactionModel> pendingTrasactions = await applicationService.GetPendingTransactions();
                    Console.WriteLine(new string('=', 100));
                    Console.WriteLine("TransactionId                      \tValor");
                    Console.WriteLine(new string('=', 100));
                    foreach (var transaction in pendingTrasactions)
                    {
                        Console.WriteLine("{0}\t{1}", transaction.TransactionId, transaction.Ammount);
                    }
                    Console.WriteLine();
                    break;
                }
                case 5:
                {
                    Console.WriteLine("Digite o Id da transferência");
                    string transactionId = Console.ReadLine();
                    if (await applicationService.CommitTransaction(transactionId))
                    {
                        Console.WriteLine("Transação efetivada");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao efetivar transação");
                    }
                    break;
                }
            }
        }
    }
}