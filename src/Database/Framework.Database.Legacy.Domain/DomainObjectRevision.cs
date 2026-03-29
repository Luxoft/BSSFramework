namespace Framework.Database.Domain;

public class DomainObjectRevision<TIdent>(TIdent identity) : DomainObjectRevisionBase<TIdent, DomainObjectRevisionInfo<TIdent>>(identity);
