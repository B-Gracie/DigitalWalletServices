using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.DAL.Entities;
public enum TxnType
{
    Deposit, Withdrawal
} 

[Table("Transactions", Schema = "Users")]

public record AccountTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public string AccountNum { get; set; }
    
    public DateTime TxnTime { get; set; }
    
    public decimal TxnAmount { get; set; }
    public decimal Balance { get; set; }

    public virtual Customer Customer { get; set; }
}

// public class AccountTransaction
// {
//     [Key]
//     public Guid TxnId { get; set; }
//     public string AccountNum { get; set; }
//     public DateTime TxnTime { get; set; }
//     //public string TxnType { get; set; }
//     public decimal TxnAmount { get; set; }
//     public decimal Balance { get; set; }
//
// }