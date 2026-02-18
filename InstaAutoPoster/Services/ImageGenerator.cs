using SkiaSharp;

namespace InstaAutoPoster.Services;

public class ImageGenerator
{
    public string CreateImage(string text)
    {
        var templates = Directory.GetFiles("templates");
        var randomTemplate = templates[new Random().Next(templates.Length)];

        using var bitmap = SKBitmap.Decode(randomTemplate);
        using var canvas = new SKCanvas(bitmap);

        var paint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 60,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };

        var lines = text.Split('\n');
        float y = bitmap.Height / 3;

        foreach (var line in lines)
        {
            canvas.DrawText(line.Trim(), bitmap.Width / 2, y, paint);
            y += 80;
        }

        var fileName = $"post_{DateTime.Now:yyyyMMddHHmmss}.jpg";

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);

        File.WriteAllBytes(fileName, data.ToArray());

        return Path.GetFullPath(fileName);
    }
}
