namespace Wallet.DAL.Entities;

public class WithdrawalResponseModel
{
        public decimal WithdrawalAmount { get; set; }
        public DateTime TxnTime { get; set; }
        public decimal UpdatedBalance { get; set; }

    }