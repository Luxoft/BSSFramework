using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.WebApi.Utils.SL
{
    public class SLJsonCompatibilityResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any())
            {
                context.HttpContext.Request.EnableBuffering();

                context.HttpContext.Items[nameof(JsonDocument)] = await JsonDocument.ParseAsync(context.HttpContext.Request.Body);

                context.HttpContext.Request.Body.Position = 0;
            }

            await next();
        }
    }
}
