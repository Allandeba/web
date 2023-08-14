using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class ContactModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ContactId { get; set; }

    [Required]
    [MaxLength(50)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format.")]
    public string Email { get; set; } = String.Empty;

    [Required]
    [MaxLength(15)]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone format.")]
    public string Phone { get; set; } = String.Empty;

    public int PersonId { get; set; }
    public virtual PersonModel? Person { get; set; }
}
