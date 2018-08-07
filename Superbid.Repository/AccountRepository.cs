using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Superbid.Domain.DomainModels;

namespace Superbid.Repository
{
    public class AccountRepository
    {
        private readonly SuperBidDbContext _superBidDbContext;
        public AccountRepository(SuperBidDbContext superBidDbContext)
        {
            _superBidDbContext = superBidDbContext;
        }
        public Account findByAccountId(string accountId)
        {
            return _superBidDbContext.Account.FirstOrDefault(x => x.Id == ObjectId.Parse(accountId));
        }

        public IList<Account> GetAccounts()
        {
            return _superBidDbContext.Account.ToArray();
        }

        public Account Create(Account account)
        {
            _superBidDbContext.Add(account);
            _superBidDbContext.SaveChanges();
            return account;
        }

        public Account Update(Account account)
        {
            _superBidDbContext.Update(account);
            _superBidDbContext.SaveChanges();
            return account;
        }

        public Account InsertAccountTransaction(string accountId, CreditTransaction transaction)
        {
            var account = findByAccountId(accountId);
            if (account == null)
                throw new InvalidOperationException("Account not found");
            account.AddTransaction(transaction);
            _superBidDbContext.Update(account);
            _superBidDbContext.SaveChanges();
            return account;
        }

        public IList<Account> findAccountByTransactionId(string transactionId)
        {
            var accounts = _superBidDbContext.Account.Where(x => x.Transactions.Any(y => y.TransactionId == ObjectId.Parse(transactionId)));

            return accounts.ToList();
        }

        public IList<Account> findAccountsWithPendingTransactions()
        {
            var accounts = _superBidDbContext.Account.Where(x => x.Transactions.Any(y => y.Pending));
            return accounts.ToList();
        }
    }
}