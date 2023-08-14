using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;
using getQuote.Framework;

namespace getQuote.Models;

public class ItemModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemId { get; set; }

    [Required]
    [Display(Name = "Item name")]
    [MaxLength(50)]
    public string ItemName { get; set; } = String.Empty;

    [Required]
    [Precision(18, 2)]
    [DataType(DataType.Currency, ErrorMessage = "Invalid currency format.")]
    public decimal Value { get; set; } = 0;

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = String.Empty;

    [NotMapped]
    [Display(Name = "Upload Image")]
    [DataType(DataType.Upload)]
    public List<IFormFile> ImageFiles { get; set; } = new();

    [NotMapped]
    [Display(Name = "Default image")]
    public String DefaultImage { get; set; } = String.Empty;

    [NotMapped]
    public List<int>? IdImagesToDelete { get; set; } = new();

    public void SetItemImageList()
    {
        if (this.ImageFiles != null && this.ImageFiles.Count > 0)
        {
            this.ItemImageList = new List<ItemImageModel>();
            foreach (var image in this.ImageFiles)
            {
                ItemImageModel itemImage = new ItemImageModel();
                itemImage.FileName = image.FileName;
                itemImage.ImageFile = ImageUtils.ResizeImage(image).ToArray();

                this.ItemImageList.Add(itemImage);
            }
        }
    }

    public void SetDefaultImage(String defaultImageFileName)
    {
        if (this.ItemImageList != null && this.ItemImageList.Count > 0)
        {
            foreach (var image in this.ItemImageList)
            {
                if (defaultImageFileName == SelectDefault.None.ToString())
                {
                    image.Main = false;
                }
                else
                {
                    image.Main = image.FileName == defaultImageFileName;
                }
            }
        }
    }

    public virtual List<ItemImageModel>? ItemImageList { get; set; }
    public virtual List<ProposalContentModel>? ProposalContent { get; set; }

    public ItemImageModel GetMainImage()
    {
        ItemImageModel? itemImageModel = ItemImageList.FirstOrDefault(a => a.Main);
        return itemImageModel;
    }
}
