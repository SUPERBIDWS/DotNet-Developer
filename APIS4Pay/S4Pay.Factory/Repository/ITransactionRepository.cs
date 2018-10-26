using System;
using System.Collections.Generic;
using System.Dynamic;
using S4Pay.Domain.Account;

namespace S4Pay.Factory.Repository
{
    public interface ITransactionRepository
    {
        bool Save(Transaction transaction, bool pending = true);
        bool Approve(Guid transactionId);
        bool Disapprove(Guid transactionId);
        ExpandoObject Get(Guid transactionId);
        IEnumerable<ExpandoObject> GetTransactionPending(Guid userId);
        IEnumerable<ExpandoObject> GetTransactionApproved(Guid userId);
    }
}