using System;

namespace S4Pay.Domain.Account
{
    public class Transaction
    {
        public Transaction()
        {
        }

        public Transaction(Guid id, Guid userId, Guid anotherUserId, bool pending, decimal value)
        {
            Id = id;
            UserId = userId;
            AnotherUserId = anotherUserId;
            Pending = pending;
            Value = value;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AnotherUserId { get; set; }
        public bool Pending { get; set; }
        public decimal Value { get; set; }

        public Transaction CreateRule(bool rule)
        {
            var signal = rule ? -1 : 1;
            return new Transaction(Id, UserId, AnotherUserId, Pending, Value * signal);
        }
    }
}