using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.DAL.Entities;

[Table("Transactions", Schema = "Users")]
public class AccountTransaction
{
  
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string AccountNum { get; set; }
        public decimal Balance { get; set; }
        public decimal DepositAmount { get; set; }

        public decimal WithdrawalAmount { get; set; }
        public DateTime TxnTime { get; set; }

}



