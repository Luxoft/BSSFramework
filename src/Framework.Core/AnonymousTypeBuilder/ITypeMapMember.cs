using System;

namespace Framework.Core
{
    public interface ITypeMapMember
    {
        string Name { get; }

        Type Type { get; }
    }
}