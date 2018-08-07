using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Superbid.ConsoleApp.AppModel;
using Superbid.Domain.DomainModels;

namespace Superbid.ConsoleApp
{
    public class ApplicationService
    {
        static HttpClient client = new HttpClient();

        public ApplicationService(string url)
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IList<AccountModel>> GetAccounts()
        {
            HttpResponseMessage response = await client.GetAsync("api/account");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IList<AccountModel>>();
            }

            return null;
        }
        
        public async Task<AccountModel> CreateAccount()
        {
            Console.WriteLine("Digite o valor do depósito inicial eg 10.50");
            var strInitalDeposit = Console.ReadLine();
            strInitalDeposit = strInitalDeposit.Replace(',', '.');
            decimal initialDeposit;
            if (decimal.TryParse(strInitalDeposit, out initialDeposit))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync<AccountCreationModel>("api/account", new AccountCreationModel(){InitialAmmount = initialDeposit});
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AccountModel>();
                }
            }
            else
            {
                Console.WriteLine(
                    "Valor do depósito inicial inválido, digite novamente o exit para sair da opção");
            }

            return null;
        }

        public async Task<AccountModel> TransferAmmount()
        {
            Console.WriteLine("Digite o Id da conta de débito:");
            string debitAccount = Console.ReadLine();
            
            Console.WriteLine("Digite o Id da conta de crédito:");
            string creditAccount = Console.ReadLine();
            
            Console.WriteLine("Digite o valor da transferência:");
            string strAmount = Console.ReadLine();
            strAmount = strAmount.Replace(',', '.');

            decimal transferAmount;
            if (decimal.TryParse(strAmount, out transferAmount))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync<TransferModel>("api/transfer", new TransferModel()
                {
                    DebitAccountId = debitAccount,
                    CreditAccountId = creditAccount,
                    Ammount = transferAmount
                });
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AccountModel>();
                }
            }

            return null;
            
        }
        
        public async Task<IList<TransactionModel>> GetPendingTransactions()
        {
            HttpResponseMessage response = await client.GetAsync("api/Transaction");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IList<TransactionModel>>();
            }

            return null;
        }

        public async Task<bool> CommitTransaction(string transactionId)
        {
            string url = string.Format("api/Transfer/{0}", transactionId);
            HttpResponseMessage response = await client.PutAsync(url, null);
            return response.IsSuccessStatusCode;
        }
    }
}