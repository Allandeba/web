using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class PersonModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int PersonId { get; set; }

    [Required]
    [Display(Name = "First name")]
    [MaxLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Last name")]
    [MaxLength(100)]
    public string? LastName { get; set; }

    [Display(Name = "Creation date")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
    public DateTime CreationDate { get; set; }

    [Required]
    [Display(Name = "Documents")]
    public virtual ICollection<DocumentModel>? Documents { get; set; }

    [Required]
    [Display(Name = "Contacts")]
    public virtual ICollection<ContactModel>? Contacts { get; set; }

    public string PersonName { get { return this.FirstName + ' ' + this.LastName; } }

}