using Microsoft.AspNetCore.Builder;

namespace Framework.WebApi.Utils
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Turn on default errors handling behavior for web requests.
        /// Error will be converted to JSON Web Response. 
        /// </summary>
        public static IApplicationBuilder UseDefaultExceptionsHandling(this IApplicationBuilder builder) => builder.UseMiddleware<ErrorHandling>();

        /// <summary>
        /// Adds correlation id to web request headers and to logs.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pattern">A pattern for correlation id. The pattern should contain single param. Example: SampleSystem_{0}</param>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder, string pattern) =>
            builder.UseMiddleware<CorrelationIdHandling>(pattern);

        /// <summary>
        /// Adds remote IP address to logs.
        /// </summary>
        public static IApplicationBuilder UseClientIpLog(this IApplicationBuilder builder) => builder.UseMiddleware<ClientIpHandling>();
    }
}
