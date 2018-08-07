using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Superbid.Domain.DomainModels
{
    [ComplexType]
    [BsonKnownTypes(typeof(DebitTransaction), typeof(CreditTransaction))]
    public class Transaction
    {
        public Transaction(ObjectId transactionId,
            decimal ammount,
            string description)
        {
            TransactionId = transactionId;
            Ammount = ammount;
            Description = description;
            TransactionDate = DateTime.UtcNow;
            Pending = true;
        }
        public ObjectId TransactionId { get; set; }
        public decimal Ammount { get; set; }
        public string TransactionType { get; }
        public bool Pending { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; private set; }
    }
}