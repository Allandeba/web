using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats.Png;

namespace getQuote.Framework;

public class ImageUtils
{
    public static MemoryStream ResizeImage(IFormFile imageFile, int width, int height)
    {
        var image = Image.Load(imageFile.OpenReadStream());
        image.Mutate(
            x =>
                x.Resize(width, height, KnownResamplers.Lanczos3).BackgroundColor(Color.Transparent)
        );

        MemoryStream outputStream = new();
        image.Save(outputStream, new PngEncoder());
        outputStream.Position = 0;

        return outputStream;
    }
}
