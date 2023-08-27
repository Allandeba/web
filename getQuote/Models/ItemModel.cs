﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using getQuote.Framework;

namespace getQuote.Models;

public class ItemModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Item name")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string ItemName { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Range(0, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Precision(18, 2)]
    public decimal Value { get; set; } = 0;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(250, ErrorMessage = Messages.MaxLengthValidation)]
    public string Description { get; set; } = String.Empty;

    [NotMapped]
    [Display(Name = "Upload Image")]
    [DataType(DataType.Upload, ErrorMessage = Messages.InvalidFormatValidation)]
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
                itemImage.ImageFile = ImageUtils
                    .ResizeImage(image, Constants.MaxImageWidth, Constants.MaxImageHeight)
                    .ToArray();

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
