using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class ProposalContentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalContentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be more there zero.")]
    [Required]
    public int Quantity { get; set; } = 1;

    public int ProposalId { get; set; }
    public virtual required ProposalModel? Proposal { get; set; }

    public int ItemId { get; set; }
    public virtual required ItemModel? Item { get; set; }
}
