using System.Collections.Generic;
using Superbid.Domain.DomainModels;

namespace Superbid.Domain.ServicesInterfaces
{
    public interface IAccountService
    {
        Account GetAccountById(string accountId);
        IList<Account> GetAccounts();
        void TransferAmmountBetweenAccounts(string debitAccountId, string creditAccountId, decimal transferAmmount);
        Account CreateAccount(decimal initialAmmount);
        IList<Account> ConfirmTransaction(string transactionId);
        IList<Transaction> GetPendingTransactions();
    }
}