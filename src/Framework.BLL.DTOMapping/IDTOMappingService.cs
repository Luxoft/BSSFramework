using Framework.Application.Repository;

namespace Framework.BLL.DTOMapping;

public interface IDTOMappingService<in TPersistentDomainObjectBase, in TIdent>
{
    TDomainObject GetById<TDomainObject>(TIdent ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
            where TDomainObject : class, TPersistentDomainObjectBase;
}
