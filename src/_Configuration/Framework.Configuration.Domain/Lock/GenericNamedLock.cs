using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Объект, с помощью которого можно реализовать пессимистическую блокировку в базе данных
/// </summary>
[BLLRole]
[UniqueGroup]
public class GenericNamedLock : BaseDirectory;
