using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Models;

public class ItemModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemId { get; set; }

    [Required]
    [Display(Name = "Item name")]
    [MaxLength(50)]
    public string? ItemName { get; set; }

    [Required]
    [Display(Name = "Value")]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal? Value { get; set; }

    [NotMapped]
    [Display(Name = "Upload Image")]
    [DataType(DataType.Upload)]
    public List<IFormFile>? ImageFiles { get; set; } = null;

    public void SetItemImageList()
    {
        if (this.ImageFiles != null && this.ImageFiles.Count > 0)
        {
            this.ItemImageList = new List<ItemImageModel>();
            foreach (var image in this.ImageFiles)
            {
                ItemImageModel itemImage = new ItemImageModel();
                itemImage.FileName = image.FileName;

                var ms = new MemoryStream();
                image.CopyTo(ms);
                itemImage.ImageFile = ms.ToArray();

                // Temporary
                itemImage.Principal = true;

                this.ItemImageList.Add(itemImage);
            }
        }
    }

    public virtual List<ItemImageModel>? ItemImageList { get; set; } = null;
}
