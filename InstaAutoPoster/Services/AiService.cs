using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;

namespace InstaAutoPoster.Services;

public class AiService
{
    private readonly Kernel _kernel;

    public AiService()
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(
            "phi3:mini",
            new Uri("http://localhost:11434"));

        _kernel = builder.Build();
    }

    public async Task<string> GenerateContent()
    {
        var rules = await File.ReadAllTextAsync("rules.md");

        string previous = File.Exists("usedPosts.txt")
            ? await File.ReadAllTextAsync("usedPosts.txt")
            : "";

        var prompt = $"""
Follow STRICTLY:

{rules}

Avoid repeating:
{previous}

Return EXACT format:

POST:
<quote>

CAPTION:
<engaging caption>

HASHTAGS:
<12 targeted hashtags>
""";

        var result = await _kernel.InvokePromptAsync(prompt);

        return result.ToString();
    }
}
