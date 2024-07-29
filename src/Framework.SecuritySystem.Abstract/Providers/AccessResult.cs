namespace Framework.SecuritySystem;

public abstract record AccessResult
{
    public record AccessGrantedResult : AccessResult
    {
        public override AccessResult Or(AccessResult otherAccessResult) => this;

        public override AccessResult And(AccessResult otherAccessResult) => otherAccessResult;


        public static readonly AccessGrantedResult Default = new();
    }

    public record AccessDeniedResult : AccessResult
    {
        public SecurityRule? SecurityRule { get; init; }

        public string? CustomMessage { get; init; }

        public (object DomainObject, Type DomainObjectType)? DomainObjectInfo { get; init; }


        public override AccessResult Or(AccessResult otherAccessResult) => otherAccessResult;

        public override AccessResult And(AccessResult otherAccessResult) => this;


        public static AccessDeniedResult Create<TDomainObject>(TDomainObject domainObject, SecurityRule? securityRule = null) =>
            new() { DomainObjectInfo = (domainObject, typeof(TDomainObject))!, SecurityRule = securityRule };

        public static readonly AccessDeniedResult Default = new();
    }

    public abstract AccessResult And(AccessResult otherAccessResult);

    public abstract AccessResult Or(AccessResult otherAccessResult);
}
