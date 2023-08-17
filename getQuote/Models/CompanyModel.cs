using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using getQuote.Framework;

namespace getQuote.Models;

public class CompanyModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int CompanyId { get; set; }

    [Required]
    [Display(Name = "Company name")]
    [MaxLength(50)]
    public string CompanyName { get; set; } = String.Empty;

    [Required]
    [MaxLength(50)]
    public string CNPJ { get; set; } = String.Empty;

    [Required]
    [MaxLength(150)]
    public string Address { get; set; } = String.Empty;

    [Required]
    [MaxLength(50)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format.")]
    public string Email { get; set; } = String.Empty;

    [Required]
    [MaxLength(15)]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone format.")]
    public string Phone { get; set; } = String.Empty;

    public byte[]? ImageFile { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Company image")]
    [DataType(DataType.Upload)]
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
