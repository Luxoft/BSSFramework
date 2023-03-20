namespace Framework.Persistent;

/// <summary>
/// Интерфейс для базового персистентного объекта с аудитом
/// </summary>
/// <typeparam name="TIdent">Тип идента</typeparam>
public interface IAuditPersistentDomainObjectBase<out TIdent> : IIdentityObject<TIdent>, IActiveObject, IAuditObject
{
}
