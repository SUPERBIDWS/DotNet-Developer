using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RestSharp;
using S4Pay.Extension;
using S4Pay.Helper;
using S4Pay.ViewModel;

namespace S4Pay
{
    public class Program
    {
        private static RestClient _restClient;
        private static string ApiS4Pay => ConfigurationManagerHelper.GetAppSetting("API_APIS4PAY");

        private static void Initialize()
        {
            _restClient = new RestClient(ApiS4Pay);
        }
        public static void Main(string[] args)
        {
            try
            {
                Initialize();
                var userX = Guid.Parse("0b7c1c6f-e2db-4a40-b457-4473a98f77db");
                var userY = Guid.Parse("7c5c1c21-4eca-4859-ade0-677b0792e59f");

                var running = true;
                while (running)
                {
                    Console.WriteLine("Informe o comando a baixo");
                    Console.WriteLine("1 - Para Usuário X");
                    Console.WriteLine("2 - Para Usuário Y");
                    Console.WriteLine("0 - Sair");
                    Console.Write("Informe o comando: ");
                    var num = Convert.ToInt32(Console.ReadLine());
                    switch (num)
                    {
                        case 1:
                            GetUser(userX, userY, "Y");
                            break;
                        case 2:
                            GetUser(userY, userX, "X");
                            break;
                        case 0:
                            running = false;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            GenerateConsoleWriteLine("Comando inválido");
                            Console.ResetColor();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(e.Message);
                Console.ResetColor();
            }
        }

        private static void GetUser(Guid userId, Guid anotherUserId, string type)
        {
            GetBalance(userId);
            Transaction(userId, anotherUserId, type);
        }

        private static void Transaction(Guid userId, Guid anotherUserId, string type)
        {
            var running = true;
            while (running)
            {
                Console.WriteLine($"1 - Transferência para {type}");
                Console.WriteLine("2 - Aprovar Transferência");
                Console.WriteLine("3 - Saldo");
                Console.WriteLine("4 - Extrato");
                Console.WriteLine("5 - Limpar");
                Console.WriteLine("0 - Sair");
                Console.Write("Informe o comando: ");
                var num = Convert.ToInt32(Console.ReadLine());
                switch (num)
                {
                    case 1:
                        Transfer(userId, anotherUserId);
                        break;
                    case 2:
                        PendingTransaction(userId);
                        break;
                    case 3:
                        GetBalance(userId);
                        break;
                    case 4:
                        Extract(userId);
                        break;
                    case 5:
                        Console.Clear();
                        break;
                    case 0:
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        GenerateConsoleWriteLine("Comando inválido");
                        Console.ResetColor();
                        break;
                }
            }
        }


        private static void PendingTransaction(Guid userId)
        {
            var pendingRequest = new RestRequest($"transaction/pending/{userId}");
            pendingRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _restClient.Execute<ApiResponse<List<TransactionModel>>>(pendingRequest);

            if (response.Data.Status == StatusEnum.Success)
            {
                var output = response.Data.Response;
                if (output.Any())
                {
                    foreach (var transaction in output)
                    {
                        var otherValue = transaction.Value * -1;
                        Console.WriteLine("".Underscore());
                        Console.WriteLine($"Código da Transferência {transaction.Id.ToString().Substring(0, 8)}");
                        Console.WriteLine($"Transferência no valor de {otherValue:0,0.00} ");
                        Console.Write("Deseja Realizar ? (S/N):");
                        var modality = Convert.ToString(Console.ReadLine());

                        if (string.Equals(modality, "s", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Approve(transaction.Id);
                        }
                        else
                        {
                            Disapprove(transaction.Id);
                        }
                    }
                }
                else
                {
                    GenerateConsoleWriteLine("Não há Nenhuma Pendência!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(response.Data.ErrorMessage);
                Console.ResetColor();
            }

        }

        private static void Transfer(Guid userId, Guid anotherUserId)
        {
            Console.Write("Informe o valor da transferência: ");
            var value = Convert.ToDecimal(Console.ReadLine());
            if (value > 0)
            {
                var transaction = new TransactionModel { UserId = userId, AnotherUserId = anotherUserId, Value = value };
                var saveRequest = new RestRequest("transaction/save", Method.POST);
                saveRequest.AddJsonBody(transaction);
                saveRequest.AddHeader("Content-Type", "application/json; charset=utf-8");

                var response = _restClient.Execute<ApiResponse<bool>>(saveRequest);
                if (response.Data.Status == StatusEnum.Success)
                {
                    var output = response.Data.Response;
                    GenerateConsoleWriteLine(output ? "Transferência Efetivada!" : "Falha na transferência!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    GenerateConsoleWriteLine(response.Data.ErrorMessage);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine("O Valor não pode ser negativo");
                Console.ResetColor();
            }
        }
        private static void Extract(Guid userId)
        {
            var extractsRequest = new RestRequest($"transaction/extracts/{userId}");
            extractsRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _restClient.Execute<ApiResponse<List<TransactionModel>>>(extractsRequest);

            if (response.Data.Status == StatusEnum.Success)
            {
                var output = response.Data.Response;
                if (output.Any())
                {
                    foreach (var transaction in output)
                    {
                        if (transaction.Value < 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (transaction.Value > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        GenerateConsoleWriteLine($"{transaction.Id.ToString().Substring(0, 8)} - {transaction.Value:0,0.00}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    GenerateConsoleWriteLine("Não há extrato disponível");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(response.Data.ErrorMessage);
                Console.ResetColor();
            }
        }
        private static void Approve(Guid transactionId)
        {
            var approveRequest = new RestRequest($"transaction/approve/{transactionId}", Method.POST);
            approveRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _restClient.Execute<ApiResponse<bool>>(approveRequest);

            if (response.Data.Status == StatusEnum.Success)
            {
                var output = response.Data.Response;
                GenerateConsoleWriteLine(output ? "Transferência aprovado!" : "Falha ao aprovar transferência");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(response.Data.ErrorMessage);
                Console.ResetColor();
            }

        }

        private static void Disapprove(Guid transactionId)
        {
            var approveRequest = new RestRequest($"transaction/disapprove/{transactionId}", Method.POST);
            approveRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _restClient.Execute<ApiResponse<bool>>(approveRequest);

            if (response.Data.Status == StatusEnum.Success)
            {
                var output = response.Data.Response;
                GenerateConsoleWriteLine(output ? "Transferência reprovada!" : "Falha ao reprovar transferência");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(response.Data.ErrorMessage);
                Console.ResetColor();
            }

        }
        private static void GetBalance(Guid userId)
        {
            var balanceRequest = new RestRequest($"transaction/balance/{userId}");
            balanceRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _restClient.Execute<ApiResponse<decimal>>(balanceRequest);

            if (response.Data.Status == StatusEnum.Success)
            {
                var balance = response.Data.Response;
                GenerateConsoleWriteLine($"Seu saldo é: {balance:0,0.00}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                GenerateConsoleWriteLine(response.Data.ErrorMessage);
                Console.ResetColor();
            }
        }

        [DebuggerStepThrough]
        private static void GenerateConsoleWriteLine(string rule)
        {
            Console.WriteLine("".Underscore());
            Console.WriteLine(rule);
            Console.WriteLine("".Underscore());
        }
    }

    [Serializable]
    public class ApiResponse<T>
    {
        public string ErrorMessage { get; set; }
        public StatusEnum Status { get; set; }
        public T Response { get; set; }
    }

    [Serializable]
    public enum StatusEnum
    {
        Failure = 0,
        Success = 1
    }

}
