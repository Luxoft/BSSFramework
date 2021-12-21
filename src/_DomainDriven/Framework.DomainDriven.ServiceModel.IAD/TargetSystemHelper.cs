using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class TargetSystemHelper
    {
        public static readonly string WorkflowName = typeof(Framework.Workflow.Domain.PersistentDomainObjectBase).GetTargetSystemName();

        public static readonly string AuthorizationName = typeof(Framework.Authorization.Domain.PersistentDomainObjectBase).GetTargetSystemName();

        public static readonly string ConfigurationName = typeof(Framework.Configuration.Domain.PersistentDomainObjectBase).GetTargetSystemName();

    //    public static readonly ITypeResolver<string> AuthorizationTypeResolver = IgnoreNamespaceTypeResolver.FromSample(typeof(Framework.Authorization.Domain.PersistentDomainObjectBase)).WithCache().WithLock();

    //    public static readonly ITypeResolver<string> WorfklowTypeResolver = IgnoreNamespaceTypeResolver.FromSample(typeof(Framework.Workflow.Domain.PersistentDomainObjectBase)).WithCache().WithLock();

    //    public static readonly ITypeResolver<string> ConfigurationTypeResolver = IgnoreNamespaceTypeResolver.FromSample(typeof(Framework.Configuration.Domain.PersistentDomainObjectBase)).WithCache().WithLock();
    }
}