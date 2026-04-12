using Framework.Core;

namespace Framework.Database.Domain;

public record ObjectModificationInfo<TIdent>(TIdent Identity, TypeNameIdentity TypeInfo, ModificationType ModificationType, long Revision);
