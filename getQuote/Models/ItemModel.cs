using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

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
    public List<IFormFile>? ImageFiles { get; set; }

    [NotMapped]
    [Required]
    [Display(Name = "Default image")]
    public String? DefaultImage { get; set; }

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
        imageToResize.Mutate(x => x.Resize(Const.MAX_IMAGE_WIDTH, Const.MAX_IMAGE_HEIGHT));

        using var outputStream = new MemoryStream();
        imageToResize.Save(outputStream, new JpegEncoder());
        outputStream.Position = 0;

        return outputStream;
    }

    [Required]
    public virtual List<ItemImageModel>? ItemImageList { get; set; }

    public ItemImageModel GetMainImage
    {
        get
        {
            ItemImageModel MainImage = this.ItemImageList.FirstOrDefault(a => a.Main);
            return MainImage;
        }
    }
}
