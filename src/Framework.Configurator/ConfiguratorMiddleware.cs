using Microsoft.AspNetCore.Http;

namespace Framework.Configurator;

public sealed class ConfiguratorMiddleware
{
    private const string StartPage = "index.html";

    private readonly RequestDelegate next;

    private readonly string route;

    public ConfiguratorMiddleware(RequestDelegate next, string route)
    {
        this.next = next;
        this.route = route;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value;
        if (this.IsStartPageRequested(path))
        {
            var content = await GetStartPageContentAsync();
            var startPageWithCorrectLocations = this.ChangeStaticLocation(content);

            await context.Response.WriteAsync(startPageWithCorrectLocations);
            return;
        }

        await this.next(context);
    }

    private bool IsStartPageRequested(string path) =>
            new[] { this.route, $"{this.route}/", $"{this.route}/{StartPage}" }
                    .Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase));

    private static Task<string> GetStartPageContentAsync()
    {
        var assembly = typeof(ConfiguratorMiddleware).Assembly;

        var resourceName = assembly
                           .GetManifestResourceNames()
                           .Single(z => z.EndsWith(StartPage, StringComparison.OrdinalIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEndAsync();
    }

    private string ChangeStaticLocation(string content) =>
            content
                    .Replace("href=\"", $"href=\"{this.route}/")
                    .Replace("src=\"", $"src=\"{this.route}/")
                    .Replace("//", "/");
}
