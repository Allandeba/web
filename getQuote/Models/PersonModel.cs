using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;

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
    public virtual DocumentModel? Document { get; set; }

    [Required]
    public virtual ContactModel? Contact { get; set; }

    public virtual ICollection<ProposalModel>? Proposal { get; set; }

    public string PersonName
    {
        get { return this.FirstName + ' ' + this.LastName; }
    }
}
