using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Superbid.Domain.DomainModels
{
    public class Account
    {
        private decimal totalBalance;
        public Account()
        {
            CreationDate = DateTime.UtcNow;
        }
        [BsonId] public ObjectId Id { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        public decimal TotalBalance
        {
            get
            {
                totalBalance = Transactions
                    .Where(x => !x.Pending)
                    .Sum(x => x.Ammount);
                return totalBalance;
            }
            set { totalBalance = value; }
        }

        public DateTime CreationDate { get; private set; }

        public void AddTransaction(Transaction transaction)
        {
            if (Transactions == null)
            {
                Transactions = new List<Transaction>();
            }

            Transactions.Add(transaction);
        }

        public Account TransferAmmountTo(Account accountCredit, decimal transferAmmount)
        {
            if (IsSameAccount(accountCredit))
            {
                throw new InvalidOperationException("you can not transfer funds to same account");
            }
            
            ObjectId transactionId = ObjectId.GenerateNewId();
            string debitDescription = string.Format("Transfered to account id {0}", Id.ToString());
            string creditDescription = string.Format("Received by account id {0}", accountCredit.Id);
            if (TotalBalance >= transferAmmount)
            {
                AddTransaction(new DebitTransaction(transactionId, transferAmmount, debitDescription));
                accountCredit.AddTransaction(new CreditTransaction(transactionId, transferAmmount, creditDescription));
                return accountCredit;
            }
            throw new InvalidOperationException("insufficient funds");
        }

        private bool IsSameAccount(Account accountCredit)
        {
            return accountCredit.Id == Id;
        }
    }
}