using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Superbid.Domain.DomainModels
{
    [ComplexType]
    [BsonDiscriminator("DEBIT")]
    public class DebitTransaction : Transaction
    {       
        public string TransactionType
        {
            get { return "DEBIT"; }
        }

        public DebitTransaction(ObjectId transactionId, decimal ammount, string description) : base(transactionId, ammount *-1, description)
        {
        }
    }
}