using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.Generation.Domain;
using Framework.Security;

namespace Framework.DomainDriven.BLLCoreGenerator;

public abstract class RootSecurityServiceGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IRootSecurityServiceGenerator
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected RootSecurityServiceGenerator(TConfiguration configuration)
            : base (configuration)
    {
    }


    protected abstract IDomainSecurityServiceGenerator GetDomainSecurityServiceGeneratorInternal(Type domainType);


    public IDomainSecurityServiceGenerator GetDomainSecurityServiceGenerator(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return this.GetDomainSecurityServiceGeneratorInternal (domainType);
    }


    public abstract IEnumerable<CodeTypeMember> GetBaseMembers();

    protected virtual bool IsSecurityServiceDomainType(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return domainType.IsSecurity();
    }

    public IEnumerable<Type> GetSecurityServiceDomainTypes()
    {
        return this.Configuration.DomainTypes.Where(this.IsSecurityServiceDomainType);
    }

    public abstract IEnumerable<CodeTypeReference> GetBLLContextBaseTypes();

    public abstract IEnumerable<CodeTypeMember> GetBLLContextMembers();

    public abstract CodeTypeReference GetGenericRootSecurityServiceType();

    public abstract CodeTypeReference GetGenericRootSecurityServiceInterfaceType();

    public abstract CodeTypeReference GetDomainInterfaceBaseServiceType();
}
