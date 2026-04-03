using Framework.Database.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain;

[UniqueGroup]
[NotAuditedClass]
public class GenericNamedLock : BaseDirectory;
