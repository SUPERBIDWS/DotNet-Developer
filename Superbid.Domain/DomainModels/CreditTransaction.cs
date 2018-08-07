using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Superbid.Domain.DomainModels
{
    [ComplexType]
    [BsonDiscriminator("CREDIT")]
    public class CreditTransaction: Transaction
    {
        
        public string TransactionType
        {
            get { return "CREDIT"; }
        }

        public CreditTransaction(ObjectId transactionId, decimal ammount, string description) : base(transactionId, ammount, description)
        {
        }
    }
}   