namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectRevision<TIdent>(TIdent identity) : DomainObjectRevisionBase<TIdent, DomainObjectRevisionInfo<TIdent>>(identity);
