using System;

namespace S4Pay.ViewModel
{
    [Serializable]
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AnotherUserId { get; set; }
        public decimal Value { get; set; }
    }
}