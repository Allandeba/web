using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

[Owned]
public class ProposalContentItems
{
    public string ItemId { get; set; }
    public string Quantity { get; set; }
}

[Owned]
public class ProposalContentJSON
{
    public List<ProposalContentItems> ProposalContentItems { get; set; } = new();

    public List<int> GetItemIds()
    {
        List<int> itemIds = new();
        foreach (var proposalContentItem in ProposalContentItems)
        {
            itemIds.Add(Int32.Parse(proposalContentItem.ItemId));
        }

        return itemIds;
    }
}

public class ProposalHistoryModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProposalHistoryId { get; set; }

    public DateTime ModificationDate { get; set; } = DateTime.MinValue;

    [Required]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Discount { get; set; } = 0;

    [Required]
    public virtual ProposalModel Proposal { get; set; }

    [Required]
    public virtual PersonModel Person { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public ProposalContentJSON ProposalContentJSON { get; set; } = new();
}
