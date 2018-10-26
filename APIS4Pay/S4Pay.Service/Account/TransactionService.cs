using System;
using System.Collections.Generic;
using System.Linq;
using S4Pay.Domain.Account;
using S4Pay.Factory.Repository;
using S4Pay.Factory.Service;

namespace S4Pay.Service.Account
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public bool Save(Transaction transaction)
        {
            return _transactionRepository.Save(transaction.CreateRule(true));
        }

        public bool Approve(Guid transactionId)
        {
            var document = _transactionRepository.Get(transactionId);
            if (document == null)
                return false;
            var dictionary = (IDictionary<string, object>)document;
            var transaction = ToTransaction(dictionary);
            var approved = _transactionRepository.Approve(transactionId);
            var saved = _transactionRepository.Save(new Transaction(Guid.NewGuid(), transaction.AnotherUserId,
                transaction.AnotherUserId, false, transaction.Value * -1), false);
            return approved && saved;
        }

        public bool Disapprove(Guid transactionId)
        {
            return _transactionRepository.Disapprove(transactionId);
        }

        public IEnumerable<Transaction> GetTransactionPending(Guid userId)
        {
            var document = _transactionRepository.GetTransactionPending(userId);
            var transactionPending =
                document?.Select(x => (IDictionary<string, object>)x).Select(ConvertToTransaction);
            return transactionPending;
        }

        public decimal GetBalance(Guid userId)
        {
            var document = _transactionRepository.GetTransactionApproved(userId);
            var balance =
                document?.Select(x => (IDictionary<string, object>)x).Select(ConvertToTransaction)
                    .Select(x => x.Value).Sum();
            return balance ?? 0;
        }

        public IEnumerable<Transaction> GetTransactionApproved(Guid userId)
        {
            var document = _transactionRepository.GetTransactionApproved(userId);
            var transactionApproved =
                document?.Select(x => (IDictionary<string, object>)x).Select(ConvertToTransaction);
            return transactionApproved;
        }

        public Func<IDictionary<string, object>, Transaction> ConvertToTransaction => dictionary =>
            new Transaction(dictionary.ContainsKey("_id") ? Guid.Parse(dictionary["_id"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("UserId") ? Guid.Parse(dictionary["UserId"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("AnotherUserId") ? Guid.Parse(dictionary["AnotherUserId"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("Pending") && Convert.ToBoolean(dictionary["Pending"].ToString()),
                dictionary.ContainsKey("Value") ? Convert.ToDecimal(dictionary["Value"].ToString()) : 0M);


        private Transaction ToTransaction(IDictionary<string, object> dictionary)
        {
            return new Transaction(dictionary.ContainsKey("_id") ? Guid.Parse(dictionary["_id"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("UserId") ? Guid.Parse(dictionary["UserId"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("AnotherUserId") ? Guid.Parse(dictionary["AnotherUserId"].ToString()) : Guid.Empty,
                dictionary.ContainsKey("Pending") && Convert.ToBoolean(dictionary["Pending"].ToString()),
                dictionary.ContainsKey("Value") ? Convert.ToDecimal(dictionary["Value"].ToString()) : 0M);
        }
    }
}