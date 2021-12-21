namespace NHibernate.Envers.Patch
{
    public class IdentityRevisionEntityInfo<TRevisionEntity, TIdentity> : IIdentityRevisionEntityInfo<TRevisionEntity, TIdentity>
    {
        public IdentityRevisionEntityInfo(TRevisionEntity revisionEntity, TIdentity identity, RevisionType operation)
        {
            this.Identity = identity;
            this.RevisionEntity = revisionEntity;
            this.Operation = operation;
        }

        public TIdentity Identity { get; private set; }

        public TRevisionEntity RevisionEntity { get; private set; }

        public RevisionType Operation { get; private set; }
    }
}
