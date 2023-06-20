using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class DocumentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int DocumentId { get; set; }

    [Required]
    [Display(Name = "Document type")]
    public DocumentTypes DocumentType { get; set; }

    [Required]
    [Display(Name = "Document")]
    [MaxLength(50)]
    public string Document { get; set; } = string.Empty;

    [ForeignKey("PersonId")]
    public int? PersonId { get; set; }
    public virtual PersonModel? Person { get; set; }
}
