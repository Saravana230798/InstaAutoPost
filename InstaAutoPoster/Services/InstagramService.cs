using System.Net.Http.Json;

namespace InstaAutoPoster.Services;

public class InstagramService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _http = new();

    public InstagramService(IConfiguration config)
    {
        _config = config;
    }

    public async Task PostAsync(string caption, string imageUrl)
    {
        var igUserId = _config["Instagram:IgUserId"];
        var accessToken = _config["Instagram:AccessToken"];

        var createResponse = await _http.PostAsync(
            $"https://graph.facebook.com/v19.0/{igUserId}/media",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "image_url", imageUrl },
                { "caption", caption },
                { "access_token", accessToken }
            }));

        var json = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        string creationId = json.id;

        await _http.PostAsync(
            $"https://graph.facebook.com/v19.0/{igUserId}/media_publish",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "creation_id", creationId },
                { "access_token", accessToken }
            }));
    }
}
