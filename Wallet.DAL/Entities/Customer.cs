using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.DAL.Entities;

[Table("Customers", Schema = "Users")]


public record Customer
{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Username { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public string Password { get; set; }
        
        public string AccountNum { get; set; }
        
        public decimal Balance { get; set; }
        
        public ICollection<AccountTransaction> Transactions { get; set; }
}







/*public record Customer
{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateCreated { get; set; }
        public string Password { get; set; }
        public string AccountNum { get; set; }
        public decimal Balance { get; set; }

}*/
