using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class ContactModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ContactId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.EmailAddress, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Email { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(25, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.PhoneNumber, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Phone { get; set; } = String.Empty;

    public int PersonId { get; set; }
    public virtual PersonModel? Person { get; set; }
}
