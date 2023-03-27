using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class SecurityPathFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SecurityPathFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.SecurityPath;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
        var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

        Func<Type, CodeTypeReference> applyGenerics = type =>

                                                              new CodeTypeReference(type)
                                                              {
                                                                      TypeArguments =
                                                                      {
                                                                              this.Configuration.Environment.PersistentDomainObjectBaseType,
                                                                              genericDomainObjectParameterTypeRef,
                                                                              this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                                                                      }
                                                              };

        var pathParameter = applyGenerics(typeof(SecurityPath<,,>)).ToParameterDeclarationExpression("securityPath");

        var currentRef = this.Configuration.GetCodeTypeReference(null, FileType.SecurityPath);

        return new CodeTypeDeclaration
               {
                       TypeParameters =
                       {
                               genericDomainObjectParameter
                       },

                       BaseTypes = { applyGenerics(typeof(SecurityPathWrapper<,,>)) },
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true,
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Private,
                                       Parameters = { pathParameter },
                                       BaseConstructorArgs = { pathParameter.ToVariableReferenceExpression() }
                               },

                               new CodeMemberMethod
                               {
                                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                       ReturnType = new CodeTypeReference("implicit operator " + currentRef.BaseType).ToTypeReference(genericDomainObjectParameterTypeRef),
                                       Parameters = { pathParameter },
                                       Statements = { currentRef.ToTypeReference(genericDomainObjectParameterTypeRef)
                                                                .ToObjectCreateExpression(pathParameter.ToVariableReferenceExpression())
                                                                .ToMethodReturnStatement() }
                               }
                       }
               };
    }
}
