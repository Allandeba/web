using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

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
    public string ProposalContentArray { get; set; } = string.Empty; // ItemsIdList
}
