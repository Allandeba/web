using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class ProposalContentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalContentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be more there zero.")]
    [Required]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; } = 1;

    public virtual ProposalModel? Proposal { get; set; }

    [NotMapped]
    public int? ItemId { get; set; }
    public virtual ItemModel? Item { get; set; }
}
