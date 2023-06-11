using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class ItemImageModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemImageId { get; set; }

    [Required]
    [Display(Name = "Principal")]
    public bool? Principal { get; set; }

    [Required]
    [Display(Name = "FileName")]
    [MaxLength(100)]
    public string? FileName { get; set; }

    [Required]
    [Display(Name = "Upload Image")]
    public byte[]? ImageFile { get; set; } = null;

    [ForeignKey("ItemId")]
    public int? ItemId { get; set; }
    public virtual ItemModel? Item { get; set; } = null;
}
