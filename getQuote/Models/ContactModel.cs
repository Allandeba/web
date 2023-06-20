using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class ContactModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ContactId { get; set; }

    [Required]
    [Display(Name = "Email")]
    [MaxLength(50)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format.")]
    public string Email { get; set; } = String.Empty;

    [Required]
    [Display(Name = "Phone")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone format.")]
    [MaxLength(15)]
    public string Phone { get; set; } = String.Empty;

    [ForeignKey("PersonId")]
    public int? PersonId { get; set; }
    public virtual PersonModel? Person { get; set; }
}
