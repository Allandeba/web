using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;

namespace getQuote.Framework;

public class ImageUtils
{
    public static MemoryStream ResizeImage(IFormFile image)
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
}
