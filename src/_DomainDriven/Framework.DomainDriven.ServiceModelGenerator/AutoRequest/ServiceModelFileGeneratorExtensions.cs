using System.CodeDom;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.DomainDriven.ServiceModelGenerator;

public static class ServiceModelFileGeneratorExtensions
{
    public static IFileGenerator<ICodeFile, CodeDomRenderer> WithAutoRequestMethods(
        this IFileGenerator<ICodeFile, CodeDomRenderer> fileGenerator)
    {
        return new AutoRequestCodeFileGenerator(fileGenerator);
    }

    private class AutoRequestCodeFileGenerator : IFileGenerator<ICodeFile, CodeDomRenderer>
    {
        private readonly IFileGenerator<ICodeFile, CodeDomRenderer> baseCodeFileGenerator;

        public AutoRequestCodeFileGenerator(IFileGenerator<ICodeFile, CodeDomRenderer> baseCodeFileGenerator)
        {
            this.baseCodeFileGenerator = baseCodeFileGenerator ?? throw new ArgumentNullException(nameof(baseCodeFileGenerator));
        }

        public CodeDomRenderer Renderer => this.baseCodeFileGenerator.Renderer;

        public IEnumerable<ICodeFile> GetFileGenerators()
        {
            foreach (var codeFile in this.baseCodeFileGenerator.GetFileGenerators())
            {
                yield return new AutoRequestFileFactory(codeFile);
            }
        }
    }

    private class AutoRequestFileFactory : ICodeFile
    {
        private readonly ICodeFile baseCodeFile;

        public AutoRequestFileFactory(ICodeFile baseCodeFile)
        {
            this.baseCodeFile = baseCodeFile ?? throw new ArgumentNullException(nameof(baseCodeFile));
        }

        public string Filename => this.baseCodeFile.Filename;

        public CodeNamespace GetRenderData()
        {
            var ns = this.baseCodeFile.GetRenderData();

            foreach (var type in ns.Types.OfType<CodeTypeDeclaration>().ToArray())
            {
                foreach (var method in type.Members.OfType<CodeMemberMethod>().Where(
                             m => m.Parameters.Count > 1 && m.Attributes.HasFlag(MemberAttributes.Public) && !(m is CodeConstructor)))
                {
                    var autoType = this.GetAutoRequestType(method);

                    var newParameter = new CodeParameterDeclarationExpression(autoType.Name, autoType.Name.ToStartLowerCase());

                    foreach (CodeParameterDeclarationExpression parameter in method.Parameters)
                    {
                        method.Statements.Insert(
                            0,
                            new CodeVariableDeclarationStatement(
                                parameter.Type,
                                parameter.Name.ToStartLowerCase(),
                                newParameter.ToVariableReferenceExpression().ToPropertyReference(parameter.Name.ToStartUpperCase())));
                    }

                    method.Parameters.Clear();
                    method.Parameters.Add(newParameter);

                    ns.Types.Add(autoType);
                }
            }

            return ns;
        }

        private CodeTypeDeclaration GetAutoRequestType(CodeMemberMethod method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            var requestType = new CodeTypeDeclaration(method.Name + "AutoRequest")
                              {
                                  Attributes = MemberAttributes.Public,
                                  IsPartial = true,
                                  IsClass = true,
                                  CustomAttributes =
                                  {
                                      typeof(DataContractAttribute).ToTypeReference().ToAttributeDeclaration(),
                                      typeof(AutoRequestAttribute).ToTypeReference().ToAttributeDeclaration()
                                  }
                              };

            method.Parameters.Cast<CodeParameterDeclarationExpression>().Foreach(
                (parameter, index) =>
                {
                    var fieldMember = new CodeMemberField
                                      {
                                          Type = parameter.Type,
                                          Name = parameter.Name.ToStartLowerCase(),
                                          Attributes = MemberAttributes.Private
                                      };

                    var fieldRefExpr = new CodeThisReferenceExpression().ToFieldReference(fieldMember);

                    var propertyMember = new CodeMemberProperty
                                      {
                                          Type = parameter.Type,
                                          Name = parameter.Name.ToStartUpperCase(),
                                          Attributes = MemberAttributes.Public,
                                          CustomAttributes =
                                          {
                                              typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration(),
                                              typeof(AutoRequestPropertyAttribute).ToTypeReference().ToAttributeDeclaration(
                                                  new CodeAttributeArgument(
                                                      nameof(AutoRequestPropertyAttribute.OrderIndex),
                                                      index.ToPrimitiveExpression()))
                                          },
                                          GetStatements = { fieldRefExpr.ToMethodReturnStatement() },
                                          SetStatements = { new CodePropertySetValueReferenceExpression().ToAssignStatement(fieldRefExpr)  }
                                      };


                    requestType.Members.Add(fieldMember);
                    requestType.Members.Add(propertyMember);
                });

            return requestType;
        }
    }
}
