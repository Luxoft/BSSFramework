using Framework.Database.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain.NLock;

[UniqueGroup]
[NotAuditedClass]
public class GenericNamedLock : BaseDirectory;
