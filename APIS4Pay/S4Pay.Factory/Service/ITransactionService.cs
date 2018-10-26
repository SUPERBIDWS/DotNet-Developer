using System;
using System.Collections.Generic;
using S4Pay.Domain.Account;

namespace S4Pay.Factory.Service
{
    public interface ITransactionService
    {
        bool Save(Transaction transaction);
        bool Approve(Guid transactionId);
        bool Disapprove(Guid transactionId);
        IEnumerable<Transaction> GetTransactionPending(Guid userId);
        decimal GetBalance(Guid userId);
        IEnumerable<Transaction> GetTransactionApproved(Guid userId);
    }
}