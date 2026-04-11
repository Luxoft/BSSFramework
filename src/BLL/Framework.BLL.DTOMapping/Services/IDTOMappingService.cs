using Framework.Database;

namespace Framework.BLL.DTOMapping.Services;

public interface IDTOMappingService<in TPersistentDomainObjectBase, in TIdent>
{
    TDomainObject? GetById<TDomainObject>(TIdent ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
            where TDomainObject : class, TPersistentDomainObjectBase;
}
