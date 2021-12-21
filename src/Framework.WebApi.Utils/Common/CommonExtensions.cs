using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.WebApi.Utils
{
    public static class CommonExtensions
    {
        internal static Dictionary<string, string> GetGlobalTags(
            this IConfiguration configuration,
            Dictionary<string, string> additional)
        {
            var tags = new Dictionary<string, string>();
            configuration.GetSection("GlobalTags").Bind(tags);

            tags.Add("Machine", Environment.MachineName);

            if (additional == null)
            {
                return tags;
            }

            foreach (var (key, value) in additional)
            {
                tags.Add(key, value);
            }

            return tags;
        }

        /// <summary>
        /// Recommended way to add MVC to project.
        /// Turns on framewrok custom logging for web requests.
        /// </summary>
        public static IMvcBuilder AddMvcBss(this IServiceCollection services) =>
            services
                .AddMvc(
                    z =>
                    {
                        z.Filters.Add<LogActionAttribute>();
                        z.EnableEndpointRouting = false;
                    });
    }
}
