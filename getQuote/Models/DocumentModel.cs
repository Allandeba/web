using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class DocumentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int DocumentId { get; set; }

    [Required]
    [Display(Name = "Document type")]
    [MaxLength(50)]
    public DocumentTypes? DocumentType { get; set; }

    [Required]
    [Display(Name = "Document")]
    [MaxLength(50)]
    public string? Document { get; set; }

    [Required]
    [Display(Name = "Person")]
    [ForeignKey("PersonId")]
    public virtual PersonModel? Person { get; set; }
}