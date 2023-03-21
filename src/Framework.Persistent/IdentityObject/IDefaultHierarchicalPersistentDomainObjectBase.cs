using System;

namespace Framework.Persistent;

public interface IDefaultHierarchicalPersistentDomainObjectBase<out T> : IHierarchicalPersistentDomainObjectBase<T, Guid>
        where T : IDefaultHierarchicalPersistentDomainObjectBase<T>
{
}
