using Quartz;
using InstaAutoPoster.Services;

namespace InstaAutoPoster;

public class InstaJob : IJob
{
    private readonly AiService _ai;
    private readonly ImageGenerator _image;
    private readonly InstagramService _instagram;

    public InstaJob(AiService ai, ImageGenerator image, InstagramService instagram)
    {
        _ai = ai;
        _image = image;
        _instagram = instagram;

    }

    public async Task Execute(IJobExecutionContext context)
    {
        var raw = await _ai.GenerateContent();

        var post = raw.Split("CAPTION:")[0]
                      .Replace("POST:", "")
                      .Trim();

        var captionPart = raw.Split("CAPTION:")[1];
        var caption = captionPart.Split("HASHTAGS:")[0].Trim();

        var hashtags = raw.Split("HASHTAGS:")[1].Trim();

        var finalCaption = caption + "\n\n" + hashtags;

        var imagePath = _image.CreateImage(post);

        // TODO: Upload image to Azure Blob here
        var publicUrl = "YOUR_PUBLIC_IMAGE_URL";

        await _instagram.PostAsync(finalCaption, publicUrl);

        await File.AppendAllTextAsync("usedPosts.txt", post + "\n\n");
    }
}
