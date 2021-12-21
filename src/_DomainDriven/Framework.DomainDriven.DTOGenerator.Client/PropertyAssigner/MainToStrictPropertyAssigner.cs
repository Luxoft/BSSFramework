using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class ClientMainToStrictPropertyAssigner<TConfiguration> : MainToStrictPropertyAssigner<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>

    {
        public ClientMainToStrictPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }


        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (this.Configuration.ClassTypes.Contains(property.PropertyType))
            {
                return new CodeNotNullConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                    {
                        this.Configuration.GetCodeTypeReference(property.PropertyType, ClientFileType.Class)
                                          .ToObjectCreateExpression(sourcePropertyRef)
                                          .ToAssignStatement(targetPropertyRef)
                    }
                };
            }

            return base.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
    }
}