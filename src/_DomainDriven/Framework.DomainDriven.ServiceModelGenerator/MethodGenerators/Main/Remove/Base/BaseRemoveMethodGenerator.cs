﻿using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class BaseRemoveMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLRemoveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly CodeTypeReference IdentTypeRef;


    protected BaseRemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.IdentTypeRef = this.Configuration
                                .Environment.ServerDTO
                                .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);
    }


    protected sealed override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    protected override SecurityRule SecurityRule { get; } = SecurityRule.Remove;
}
