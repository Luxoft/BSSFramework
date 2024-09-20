using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven;

public record ObjectModification(object Object, Type ObjectType, ModificationType ModificationType);
