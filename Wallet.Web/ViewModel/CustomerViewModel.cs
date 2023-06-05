using System.ComponentModel.DataAnnotations;

namespace Wallet.Web;

public record CustomerViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Name is Required")]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is Required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Username is Required")]
    public string Username { get; set; }

    public DateTime DateCreated { get; set; }

    public string Password { get; set; }
    public string AccountNum { get; set; }
    public decimal Balance { get; set; }



    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstName == null)
        {
            yield return new ValidationResult("Firstname is invalid", new[]
            {
                nameof(FirstName
                )
            });
        }

        if (Email == null)
        {
            yield return new ValidationResult("Email is invalid", new[] { nameof(Email) });
        }

        if (Username == null)
        {
            yield return new ValidationResult("MobileNo is invalid", new[] { nameof(Username) });
        }
    }
}
