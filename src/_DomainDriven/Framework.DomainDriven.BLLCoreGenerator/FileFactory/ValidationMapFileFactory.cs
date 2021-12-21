﻿using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class ValidationMapFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public ValidationMapFileFactory(TConfiguration configuration)
            : base(configuration, null)
        {
        }

        public override FileType FileType => FileType.ValidationMap;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.Name,
                Attributes = MemberAttributes.Public,
                IsPartial = true,
            };
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidationMapBase);
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var member in base.GetMembers())
            {
                yield return member;
            }

            {
                var extendedValidationDataParam =
                    typeof(IDynamicSource).ToTypeReference().ToParameterDeclarationExpression("extendedValidationData");


                yield return new CodeConstructor
                {
                    Attributes = MemberAttributes.Public,
                    Parameters = { extendedValidationDataParam },
                    BaseConstructorArgs = { extendedValidationDataParam.ToVariableReferenceExpression() }
                };
            }
        }
    }
}