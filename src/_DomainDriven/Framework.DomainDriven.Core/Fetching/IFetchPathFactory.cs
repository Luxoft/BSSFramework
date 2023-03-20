using System;
using System.Collections.Generic;

using Framework.Core;

namespace Framework.DomainDriven;

public interface IFetchPathFactory<in TFetchBuildRule> : IFactory<Type, TFetchBuildRule, IEnumerable<PropertyPath>>
{

}

public static class FetchPathFactoryExtensions
{
    public static IFetchPathFactory<TSource> WithCompress<TSource>(this IFetchPathFactory<TSource> fetchPathFactory)
    {
        if (fetchPathFactory == null) throw new ArgumentNullException(nameof(fetchPathFactory));

        return new FuncFetchPathFactory<TSource>((domainType, source) => fetchPathFactory.Create(domainType, source).Compress());
    }

    private class FuncFetchPathFactory<TFetchBuildRule> : FuncFactory<Type, TFetchBuildRule, IEnumerable<PropertyPath>>, IFetchPathFactory<TFetchBuildRule>
    {
        public FuncFetchPathFactory(Func<Type, TFetchBuildRule, IEnumerable<PropertyPath>> createFunc)
                : base(createFunc)
        {
        }
    }
}
