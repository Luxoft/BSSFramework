using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator
{
    public sealed class ConfiguratorMiddleware
    {
        private const string StartPage = "index.html";

        private readonly RequestDelegate _next;

        private readonly string _route;

        public ConfiguratorMiddleware(RequestDelegate next, string route)
        {
            this._next = next;
            this._route = route;
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

            await this._next(context);
        }

        private bool IsStartPageRequested(string path) =>
            new[] { this._route, $"{this._route}/", $"{this._route}/{StartPage}" }
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
                .Replace("href=\"", $"href=\"{this._route}/")
                .Replace("src=\"", $"src=\"{this._route}/")
                .Replace("//", "/");
    }
}
