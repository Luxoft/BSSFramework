using Microsoft.AspNetCore.Http;

namespace Framework.Configurator;

public sealed class ConfiguratorMiddleware(RequestDelegate next, string route)
{
    private const string StartPage = "index.html";

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.PathBase + context.Request.Path.Value!;
        if (this.IsStartPageRequested(path))
        {
            var content = await GetStartPageContentAsync();
            var startPageWithCorrectLocations = this.ChangeStaticLocation(content);

            await context.Response.WriteAsync(startPageWithCorrectLocations);
            return;
        }

        await next(context);
    }

    private bool IsStartPageRequested(string path) =>
        new[] { route, $"{route}/", $"{route}/{StartPage}" }
            .Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase));

    private static async Task<string> GetStartPageContentAsync()
    {
        var assembly = typeof(ConfiguratorMiddleware).Assembly;

        var resourceName = assembly
                           .GetManifestResourceNames()
                           .Single(z => z.EndsWith(StartPage, StringComparison.OrdinalIgnoreCase));

        await using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        return await reader.ReadToEndAsync();
    }

    private string ChangeStaticLocation(string content) =>
        content
            .Replace("href=\"", $"href=\"{route}/")
            .Replace("src=\"", $"src=\"{route}/")
            .Replace("//", "/");
}
