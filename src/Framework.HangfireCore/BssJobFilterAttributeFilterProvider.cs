using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Hangfire.Common;

namespace Framework.HangfireCore;

internal class BssJobFilterAttributeFilterProvider : JobFilterAttributeFilterProvider
{
    private readonly IDictionaryCache<Type, JobFilterAttribute[]> cache = new DictionaryCache<Type, JobFilterAttribute[]>(
        jobType =>
        {
            var innerJobType = jobType.GetGenericTypeImplementationArguments(typeof(MiddlewareJob<,>), args => args.First());

            return innerJobType.GetCustomAttributes<JobFilterAttribute>(true).ToArray();
        });

    protected override IEnumerable<JobFilterAttribute> GetTypeAttributes(Job job)
    {
        return this.cache[job.Type];
    }
}
