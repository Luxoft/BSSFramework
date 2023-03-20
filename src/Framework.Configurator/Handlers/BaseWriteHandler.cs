using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public abstract class BaseWriteHandler
{
    // TODO: this can be replaced with built serialization/deserialization
    protected async Task<TModel> ParseRequestBodyAsync<TModel>(HttpContext context)
    {
        using var streamReader = new StreamReader(context.Request.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        return JsonSerializer.Deserialize<TModel>(requestBody);
    }
}
