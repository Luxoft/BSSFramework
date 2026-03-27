using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.__Base.ByProperty;

namespace Framework.CodeGeneration.DTOGenerator.Server.Members.MapToDomainObject;

public class MainMapToDomainObjectMethodFactory<TConfiguration, TFileFactory>(TFileFactory fileFactory)
    : BaseMapToDomainObjectMethodFactory<TConfiguration, TFileFactory, MainDTOFileType>(fileFactory)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    where TFileFactory : DTOFileFactory<TConfiguration, MainDTOFileType>
{
    protected override MemberAttributes MemberAttributes => this.FileFactory.FileType.ToMapToDomainObjectMemberAttributes();


    protected override IEnumerable<CodeStatement> GetMapMethodCodeStatements()
    {
        if (this.FileFactory.FileType.HasBaseType())
        {
            yield return new CodeBaseReferenceExpression().ToMethodInvokeExpression(this.Configuration.MapToDomainObjectMethodName, this.MappingServiceParameterRefExpr, this.TargetDomainParameterRefExpr)
                                                          .ToExpressionStatement();
        }

        foreach (var baseMapMethod in base.GetMapMethodCodeStatements())
        {
            yield return baseMapMethod;
        }
    }
}
