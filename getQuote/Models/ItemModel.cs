using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;

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
    [Display(Name = "Value")]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Value { get; set; } = 0;

    [Required]
    [Display(Name = "Description")]
    [MaxLength(250)]
    public string Description { get; set; } = String.Empty;

    [NotMapped]
    [Display(Name = "Upload Image")]
    [DataType(DataType.Upload)]
    public List<IFormFile> ImageFiles { get; set; } = new();

    [NotMapped]
    [Display(Name = "Default image")]
    public String DefaultImage { get; set; } = String.Empty;

    public void SetItemImageList()
    {
        if (this.ImageFiles != null && this.ImageFiles.Count > 0)
        {
            this.ItemImageList = new List<ItemImageModel>();
            foreach (var image in this.ImageFiles)
            {
                ItemImageModel itemImage = new ItemImageModel();
                itemImage.FileName = image.FileName;
                itemImage.ImageFile = ResizeImage(image).ToArray();

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

    private MemoryStream ResizeImage(IFormFile image)
    {
        using var ms = new MemoryStream();
        image.CopyTo(ms);
        ms.Position = 0;

        using var imageToResize = Image.Load(ms);
        imageToResize.Mutate(x => x.Resize(Constants.MaxImageWidth, Constants.MaxImageHeight));

        using var outputStream = new MemoryStream();
        imageToResize.Save(outputStream, new JpegEncoder());
        outputStream.Position = 0;

        return outputStream;
    }

    public virtual List<ItemImageModel>? ItemImageList { get; set; }
    public virtual List<ProposalContentModel>? ProposalContent { get; set; }

    public ItemImageModel GetMainImage()
    {
        ItemImageModel? itemImageModel = ItemImageList.FirstOrDefault(a => a.Main);
        return itemImageModel;
    }
}
