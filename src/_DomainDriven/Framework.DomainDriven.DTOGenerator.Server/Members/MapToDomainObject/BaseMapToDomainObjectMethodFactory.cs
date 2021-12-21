using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.DomainDriven.Generation;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class BaseMapToDomainObjectMethodFactory<TConfiguration, TFileFactory, TFileType> : IMethodGenerator
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        where TFileFactory : DTOFileFactory<TConfiguration, TFileType>
        where TFileType : DTOFileType
    {
        public readonly TFileFactory FileFactory;


        public readonly CodeParameterDeclarationExpression TargetDomainParameter;

        public readonly CodeVariableReferenceExpression TargetDomainParameterRefExpr;


        public readonly CodeParameterDeclarationExpression MappingServiceParameter;

        public readonly CodeVariableReferenceExpression MappingServiceParameterRefExpr;



        public BaseMapToDomainObjectMethodFactory(TFileFactory fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            this.FileFactory = fileFactory;

            this.TargetDomainParameter = this.FileFactory.GetDomainTypeTargetParameter();
            this.TargetDomainParameterRefExpr = this.TargetDomainParameter.ToVariableReferenceExpression();

            this.MappingServiceParameter = this.FileFactory.GetMappingServiceParameter();
            this.MappingServiceParameterRefExpr = this.MappingServiceParameter.ToVariableReferenceExpression();
        }


        public IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> Configuration => this.FileFactory.Configuration;

        protected virtual MemberAttributes MemberAttributes { get; } = MemberAttributes.Public | MemberAttributes.Final;


        public CodeMemberMethod GetMethod()
        {
            return new CodeMemberMethod
            {
                Attributes = this.MemberAttributes,
                Name = this.Configuration.MapToDomainObjectMethodName,
                Parameters = { this.MappingServiceParameter, this.TargetDomainParameter },
                Statements = { this.GetMapMethodCodeStatements().Composite() }
            };
        }


        protected virtual IEnumerable<CodeStatement> GetMapMethodCodeStatements()
        {
            yield return this.MappingServiceParameterRefExpr
                             .ToMethodInvokeExpression("Map" + this.FileFactory.DomainType.Name, new CodeThisReferenceExpression(), this.TargetDomainParameterRefExpr)
                             .ToExpressionStatement();
        }
    }
}