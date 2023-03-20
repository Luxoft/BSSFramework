using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLCoreGenerator;

public abstract class DomainSecurityServiceGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IDomainSecurityServiceGenerator
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly Type DomainType;

    protected DomainSecurityServiceGenerator(TConfiguration configuration, Type domainType)
            : base(configuration)
    {
        this.DomainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.DomainTypeReference = this.DomainType.ToTypeReference();
    }

    public virtual CodeTypeReference DomainTypeReference { get; }

    public abstract CodeTypeReference BaseServiceType { get; }

    public abstract IEnumerable<CodeTypeMember> GetMembers();

    public abstract IEnumerable<CodeTypeReference> GetBaseTypes();

    public abstract IEnumerable<(CodeTypeReference ParameterType, string Name)> GetBaseTypeConstructorParameters();

    public virtual CodeConstructor GetConstructor()
    {
        var resultCtor = new CodeConstructor { Attributes = MemberAttributes.Public };

        foreach (var baseTypedParameter in this.GetBaseTypeConstructorParameters())
        {
            var resultParameter = baseTypedParameter.ParameterType.ToParameterDeclarationExpression(baseTypedParameter.Name);

            resultCtor.Parameters.Add(resultParameter);
            resultCtor.BaseConstructorArgs.Add(resultParameter.ToVariableReferenceExpression());
        }

        return resultCtor;
    }
}
