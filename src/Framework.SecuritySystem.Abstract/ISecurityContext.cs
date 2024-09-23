using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Интерфейс доменного типа авторизации для типизированного контекста.
/// </summary>
public interface ISecurityContext : IIdentityObject<Guid>;
