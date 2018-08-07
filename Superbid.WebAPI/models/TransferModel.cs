namespace Superbid.WebAPI.models
{
    public class TransferModel
    {
        public string DebitAccountId { get; set; }
        public string CreditAccountId { get; set; }
        public decimal Ammount { get; set; }
    }
}