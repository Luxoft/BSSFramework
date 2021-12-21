using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public interface IServerPropertyAssigner : IPropertyAssigner
    {
        CodeExpression MappingServiceRefExpr { get; }

        CodeExpression ContextRef { get; }

        CodeParameterDeclarationExpression DomainParameter { get; }
    }


    public abstract class ServerPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>, IServerPropertyAssigner
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        protected ServerPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }


        public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

        public CodeExpression ContextRef => this.MappingServiceRefExpr.ToPropertyReference("Context");

        public CodeParameterDeclarationExpression DomainParameter => this.DomainType.GetDomainObjectParameter();

    }
}