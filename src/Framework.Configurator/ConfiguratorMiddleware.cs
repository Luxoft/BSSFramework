using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator;

public sealed class ConfiguratorMiddleware(RequestDelegate next, string route)
{
    private const string StartPage = "index.html";

    private static readonly IReadOnlyCollection<(string Extension, string ContentType)> ContentTypes =
        [(".css", MediaTypeNames.Text.Css), (".ico", MediaTypeNames.Image.Icon), (".js", MediaTypeNames.Text.JavaScript)];

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.PathBase + context.Request.Path.Value!;
        if (path.StartsWith(route, StringComparison.OrdinalIgnoreCase))
        {
            PatchRequestPath(context, path);
            PatchContentType(context, path);

            if (this.IsStartPageRequested(path))
            {
                var content = await GetStartPageContentAsync();
                var startPageWithCorrectLocations = this.ChangeStaticLocation(content);

                await context.Response.WriteAsync(startPageWithCorrectLocations);

                return;
            }
        }

        await next(context);
    }

    private static void PatchContentType(HttpContext context, string path)
    {
        // patch Request.ContentType since running on IIS gives ContentType == null
        if (context.Response.StatusCode != (int)HttpStatusCode.OK)
        {
            return;
        }

        var contentType = ContentTypes.Where(x => path.EndsWith(x.Extension)).Select(x => x.ContentType).SingleOrDefault() ?? MediaTypeNames.Text.Html;
        context.Response.ContentType = contentType;
    }

    private static void PatchRequestPath(HttpContext context, string path)
    {
        // patch Request.Path since Microsoft.AspNetCore.StaticFiles.Helpers.TryMatchPath() ignores PathBase !!!
        context.Request.Path = new PathString(path);
        context.Request.PathBase = new PathString();
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
