using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Объект, с помощью которого можно реализовать пессимистическую блокировку в базе данных
/// </summary>
[UniqueGroup]
[NotAuditedClass]
[IgnoreHbmMapping]
public class GenericNamedLock : BaseDirectory;
