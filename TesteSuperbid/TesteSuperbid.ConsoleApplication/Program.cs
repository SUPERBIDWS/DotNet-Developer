using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TesteSuperbid.ConsoleApplication
{
    class Program
    {
        static string _baseurl = ConfigurationManager.AppSettings["baseurl"];

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Digite uma opção:");
                Console.WriteLine("1 - Para transferir");
                Console.WriteLine("2 - Para consultar saldo");
                Console.WriteLine("3 - Para confirmar transferencia");
                Console.WriteLine("4 - Para listar transferencias pendentes");
                Console.WriteLine("5 - Para limpar");
                Console.WriteLine("6 - Sair");

                string option = Console.ReadLine();
                int optionNumber = 0;

                if (Int32.TryParse(option, out optionNumber))
                {
                    switch (optionNumber)
                    {
                        case 1:
                            Response responseTransaction = PostTransaction();
                            WriteTransactionResult(responseTransaction);
                            break;
                        case 2:
                            Response ResponseAccount = GetAccount();
                            WriteAccount(ResponseAccount);
                            break;
                        case 3:
                            Response pendingTransactions = GetPendingTransactions();
                            WritePendingTransaction(pendingTransactions);

                            if (pendingTransactions.Obj != null)
                            {
                                Response responseAccoun = PostTransactionConfirmation();
                                WriteAccount(responseAccoun);
                            }
                            break;
                        case 4:
                            Response pendingTransactionsList = GetPendingTransactions();
                            WritePendingTransaction(pendingTransactionsList);
                            break;
                        case 5:
                            Console.Clear();
                            break;
                        case 6:
                            continueProgram = false;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Comando inválido!\n");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Comando inválido!\n");
                }
            }
            Console.WriteLine("Aperta qualquer tecla para fechar.");
            Console.ReadKey();
        }

        private static Response PostTransaction()
        {
            Response response = new Response();
            Transaction transaction = new Transaction();
            string json = "";
            string jsonResult = "";

            // accountFrom
            Console.WriteLine("Digite o id da conta a ser debitada: ");
            string accountFromIdStr = Console.ReadLine();
            int accountFromId = 0;

            if (!Int32.TryParse(accountFromIdStr, out accountFromId))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }
            if (accountFromId <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }

            // accountTo
            Console.WriteLine("Digite o id da conta a ser creditada: ");
            string accountToIdStr = Console.ReadLine();
            int accountToId = 0;

            if (!Int32.TryParse(accountToIdStr, out accountToId))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }
            if (accountToId <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }


            // ammout
            Console.WriteLine("Digite o valor a ser transferido: ");
            string ammoutStr = Console.ReadLine().Replace(",", ".");
            double ammount = 0D;

            if (!Double.TryParse(ammoutStr, out ammount))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }

            if (ammount <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }

            transaction.AccountFrom.Id = accountFromId;
            transaction.AccountTo.Id = accountToId;
            transaction.Ammount = ammount;
            transaction.Date = DateTime.Now;

            json = Serialize(transaction);
            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers["Content-type"] = "application/json";
                jsonResult = client.UploadString(_baseurl + "/api/Transaction", "POST", json);
            }

            response = Deserialize<Response>(jsonResult);

            return response;
        }

        private static void WriteTransactionResult(Response response)
        {
            if (!response.Error)
            {
                Console.WriteLine();
                Console.WriteLine("Transação efetuada com sucesso");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Não foi possível completar a operação");
                Console.WriteLine(response.Msg);
                Console.WriteLine();
            }
        }

        private static Response GetAccount()
        {
            Response response = new Response();
            string jsonResult = "";
            Console.WriteLine("Digite o id da conta a ser consultada: ");
            string accountIdStr = Console.ReadLine();
            int accountId = 0;

            if (!Int32.TryParse(accountIdStr, out accountId))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }

            if (accountId <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }

            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers["Content-type"] = "application/json";
                jsonResult = client.DownloadString(
                    String.Format(_baseurl + "/api/Account/{0}", accountId));
            }

            response = Deserialize<Response>(jsonResult);

            return response;
        }

        private static Response PostTransactionConfirmation()
        {
            Response response = new Response();
            string json = "";
            string jsonResult = "";

            Console.WriteLine("Digite o id da transação a ser confirmada: ");
            string pendingTransactionIdStr = Console.ReadLine();
            int pendingTransactionId = 0;

            if (!Int32.TryParse(pendingTransactionIdStr, out pendingTransactionId))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }
            if (pendingTransactionId <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }
            Transaction pendingTransaction = new Transaction();
            pendingTransaction.Id = pendingTransactionId;
            pendingTransaction.Pending = false;

            json = Serialize(pendingTransaction);
            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers["Content-type"] = "application/json";
                jsonResult = client.UploadString(
                    String.Format(_baseurl + "/api/Transaction/Confirm/{0}", pendingTransactionId), "POST", "");
            }

            return Deserialize<Response>(jsonResult);
        }

        public static void WriteAccount(Response response)
        {
            if (!response.Error)
            {
                Account account = Deserialize<Account>(response.Obj.ToString());
                Console.WriteLine();
                Console.WriteLine(String.Format("Id da conta: {0}", account.Id));
                Console.WriteLine(String.Format("Saldo: R$ {0}", account.Balance));
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(response.Msg);
                Console.WriteLine();
            }
        }

        private static Response GetPendingTransactions()
        {
            Response response = new Response();
            List<Transaction> pendingTransactionsList = new List<Transaction>();
            string jsonResult = "";
            Console.WriteLine("Digite o id da conta: ");
            string accountIdStr = Console.ReadLine();
            int accountId = 0;

            if (!Int32.TryParse(accountIdStr, out accountId))
            {
                response.Error = true;
                response.Msg = "Valor incorreto";
                return response;
            }

            if (accountId <= 0)
            {
                response.Error = true;
                response.Msg = "Valor menor que 0";
                return response;
            }

            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers["Content-type"] = "application/json";
                jsonResult = client.DownloadString(
                    String.Format(_baseurl + "/api/Transaction/pending/{0}", accountId));
            }

            response = Deserialize<Response>(jsonResult);

            return response;
        }

        private static void WritePendingTransaction(Response response)
        {

            if (!response.Error)
            {
                IList<Transaction> pendingTransactionsList = Deserialize<IList<Transaction>>(response.Obj.ToString());

                if (pendingTransactionsList.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine("Transações pendentes da conta: ");
                    Console.WriteLine();
                    foreach (var item in pendingTransactionsList)
                    {
                        Console.WriteLine(
                            String.Format("Id - {0}, Valor - R$ {1}, Id conta creditada: {2}, Data - {3}",
                            item.Id, item.Ammount, item.AccountTo.Id, item.Date));
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Conta não contém transações pendentes");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(response.Msg);
                Console.WriteLine();
            }

        }

        public static T Deserialize<T>(string json)
        {
            var value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        public static string Serialize(object obj)
        {
            var value = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return value;
        }
    }
}
