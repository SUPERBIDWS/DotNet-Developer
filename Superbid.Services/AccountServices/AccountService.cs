using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Superbid.Domain.DomainModels;
using Superbid.Domain.ServicesInterfaces;
using Superbid.Repository;

namespace Superbid.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService(AccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        public Account GetAccountById(string accountId)
        {
            var account = _accountRepository.findByAccountId(accountId);
            
            return account;
        }
        

        public IList<Account> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public void TransferAmmountBetweenAccounts(string debitAccountId, string creditAccountId, decimal transferAmmount)
        {
            var debitAccount = GetAccountById(debitAccountId);
            var creditAccount = GetAccountById(creditAccountId);
            debitAccount.TransferAmmountTo(creditAccount, transferAmmount);
            _accountRepository.Update(debitAccount);
            _accountRepository.Update(creditAccount);
        }

        public Account CreateAccount(decimal initialAmmount)
        {
            var account = new Account();
            if (initialAmmount > 0)
            {
                var creditTransaction =
                    new CreditTransaction(ObjectId.GenerateNewId(), initialAmmount, "Initial Deposit");
                creditTransaction.Pending = false;
                account.AddTransaction(creditTransaction);
            }
            return _accountRepository.Create(account);
        }

        public IList<Account> ConfirmTransaction(string transactionId)
        {
            IList<Account> pendingAccounts = this._accountRepository.findAccountByTransactionId(transactionId);
            foreach (var account in pendingAccounts)
            {
                CheckAndApproveTransaction(account, ObjectId.Parse(transactionId));
            }
            return pendingAccounts;
        }

        private void CheckAndApproveTransaction(Account account, ObjectId transactionId)
        {
            foreach (var transaction in account.Transactions)
            {
                if (transaction.TransactionId == transactionId && transaction.Pending)
                {
                    transaction.Pending = false;
                    _accountRepository.Update(account);
                }
            }
        }

        public IList<Transaction> GetPendingTransactions()
        {
            IList<Account> pendingAccounts = this._accountRepository.findAccountsWithPendingTransactions();

            var pendingTransactions = new List<Transaction>();
            foreach (var account in pendingAccounts)
            {
                foreach (var transaction in account.Transactions.FindAll(x => x.Pending))
                {
                    if (pendingTransactions.All(x => x.TransactionId != transaction.TransactionId))
                    {
                        pendingTransactions.Add(transaction);
                    } 
                }
            }

            return pendingTransactions;

        }
        
    }
}