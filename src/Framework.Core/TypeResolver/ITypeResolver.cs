using System;

namespace Framework.Core
{
    public interface ITypeResolver<in T> : ITypeSource
    {
        Type Resolve(T identity);
    }
}