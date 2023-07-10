using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class ProposalModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalId { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
    public DateTime ModificationDate { get; set; } = DateTime.MinValue;

    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Discount { get; set; } = 0;

    [Display(Name = "Person")]
    public int PersonId { get; set; } = 0;
    public virtual required PersonModel? Person { get; set; }

    public virtual required List<ProposalContentModel> ProposalContent { get; set; }

    public List<int> GetIdProposalContentList()
    {
        List<int> proposalContentIdList = new();
        foreach (var ProposalContent in this.ProposalContent)
        {
            proposalContentIdList.Add(ProposalContent.ProposalContentId);
        }

        return proposalContentIdList;
    }

    public List<int> GetItemsIdList()
    {
        return ProposalContent.Select(pc => pc.ItemId).ToList();
    }
}
