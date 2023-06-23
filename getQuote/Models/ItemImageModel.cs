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
    [Display(Name = "Main")]
    public bool Main { get; set; } = false;

    [Required]
    [Display(Name = "FileName")]
    [MaxLength(100)]
    public string FileName { get; set; } = String.Empty;

    [Display(Name = "Upload Image")]
    public byte[]? ImageFile { get; set; }

    [ForeignKey("ItemId")]
    public int? ItemId { get; set; }
    public virtual ItemModel? Item { get; set; }
}
