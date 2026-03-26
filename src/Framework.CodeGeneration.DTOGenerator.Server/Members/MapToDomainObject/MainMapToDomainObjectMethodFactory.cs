using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class MainMapToDomainObjectMethodFactory<TConfiguration, TFileFactory> : BaseMapToDomainObjectMethodFactory<TConfiguration, TFileFactory, MainDTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        where TFileFactory : DTOFileFactory<TConfiguration, MainDTOFileType>
{
    public MainMapToDomainObjectMethodFactory(TFileFactory fileFactory)
            : base(fileFactory)
    {
    }


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
