using System.Reflection;

using Anch.Core;
using Anch.Core.DictionaryCache;

using Hangfire.Common;

namespace Framework.Infrastructure.Hangfire;

internal class BssJobFilterAttributeFilterProvider : JobFilterAttributeFilterProvider
{
    private readonly IDictionaryCache<Type, JobFilterAttribute[]> cache = new DictionaryCache<Type, JobFilterAttribute[]>(jobType =>
    {
        var innerJobType = jobType.GetGenericTypeImplementationArguments(typeof(MiddlewareJob<,>), args => args.First())!;

        return innerJobType.GetCustomAttributes<JobFilterAttribute>(true).ToArray();
    }).WithLock();

    protected override IEnumerable<JobFilterAttribute> GetTypeAttributes(Job job) => this.cache[job.Type];
}
