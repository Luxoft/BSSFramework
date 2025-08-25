using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Projection;
using Framework.Security;
using SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class EnumRootSecurityServiceGenerator<TConfiguration> : RootSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public EnumRootSecurityServiceGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }

    protected override IDomainSecurityServiceGenerator GetDomainSecurityServiceGeneratorInternal(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var dependencySecurityAttr = domainType.GetCustomAttribute<DependencySecurityAttribute>();

        if (dependencySecurityAttr == null)
        {
            return new EnumDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType);
        }
        else
        {
            if (dependencySecurityAttr.IsUntyped)
            {
                return new UntypedDependencyDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType, dependencySecurityAttr);
            }
            else
            {
                return new DependencyDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType, dependencySecurityAttr);
            }
        }
    }

    public override IEnumerable<CodeTypeMember> GetBaseMembers()
    {
        return from domainType in this.Configuration.SecurityServiceDomainTypes

               where !domainType.IsProjection()

               where !domainType.HasAttribute<DependencySecurityAttribute>()

               let typeParameters = this.Configuration.GetDomainTypeSecurityParameters(domainType).ToArray()

               let domainTypeRef = typeParameters.Select(p => p.ToTypeReference()).FirstOr(() => domainType.ToTypeReference())

               select new CodeMemberMethod
                      {
                              Name = domainType.ToGetSecurityPathMethodName(),
                              Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                              ReturnType = typeof(SecurityPath<>).ToTypeReference(domainTypeRef),
                      }.WithTypeParameters(typeParameters);
    }

    public override IEnumerable<CodeTypeReference> GetBLLContextBaseTypes()
    {
        yield break;
    }

    public override CodeTypeReference GetGenericRootSecurityServiceType()
    {
        return typeof(RootSecurityService<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference());
    }

    public override CodeTypeReference GetGenericRootSecurityServiceInterfaceType()
    {
        return typeof(IRootSecurityService<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference());
    }
}
