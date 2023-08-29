using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using getQuote.Framework;

namespace getQuote.Models;

public class CompanyModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Company name")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string CompanyName { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(20, ErrorMessage = Messages.MaxLengthValidation)]
    public string CNPJ { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(150, ErrorMessage = Messages.MaxLengthValidation)]
    public string Address { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.EmailAddress, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Email { get; set; } = String.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(25, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.PhoneNumber, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Phone { get; set; } = String.Empty;

    public byte[]? ImageFile { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Company image")]
    [DataType(DataType.Upload, ErrorMessage = Messages.InvalidFormatValidation)]
    public IFormFile? FormImageFile { get; set; }

    public void SetNewImage()
    {
        if (FormImageFile != null)
        {
            ImageFile = ImageUtils
                .ResizeImage(
                    FormImageFile,
                    Constants.MaxImageLogoWidth,
                    Constants.MaxImageLogoHeight
                )
                .ToArray();
        }
    }
}
