using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.ProjectionGenerator.Configuration;
using Framework.CodeGeneration.ProjectionGenerator.Extensions;
using Framework.Core;
using Framework.Projection;

namespace Framework.CodeGeneration.ProjectionGenerator.FileFactory;

public class CustomProjectionFileFactoryBase<TConfiguration>(TConfiguration configuration, Type domainType)
    : CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IProjectionGeneratorConfiguration<IProjectionGenerationEnvironment>
{
    public override FileType FileType { get; } = FileType.CustomProjectionBase;

    public override CodeTypeReference BaseReference => this.DomainType.BaseType.ToTypeReference(); // this.Configuration.Environment.GetProjectionBaseType(this.DomainType).ToTypeReference();


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
            IsPartial = true,
        };

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        {
            var projectionAttr = this.DomainType.GetCustomAttribute<ProjectionAttribute>();

            if (projectionAttr != null)
            {
                yield return projectionAttr.ToAttributeDeclaration();
            }
        }
    }

    private IEnumerable<PropertyInfo> GetProperties(bool includeBase) => this.Configuration.Environment.GetProjectionProperties(this.DomainType!, includeBase, true);

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        foreach (var property in this.GetProperties(false))
        {
            var genProp = this.CreateCustomProperty(property);

            genProp.CustomAttributes.AddRange(this.Configuration.GetPropertyAttributeDeclarations(property).ToArray());

            yield return genProp;
        }
    }

    private CodeMemberProperty CreateCustomProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return new CodeMemberProperty
               {
                       Name = property.Name,

                       Type = property.PropertyType.ToTypeReference(),

                       Attributes = MemberAttributes.Public | MemberAttributes.Abstract,

                       HasGet = true,

                       HasSet = property.HasSetMethod()
               };
    }


    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        yield return new CodeConstructor
                     {
                             Attributes = MemberAttributes.Family
                     };
    }
}
