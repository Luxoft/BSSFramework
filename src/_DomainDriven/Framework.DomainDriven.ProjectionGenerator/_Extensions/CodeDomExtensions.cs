using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Projection;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Transfering;

namespace Framework.DomainDriven.ProjectionGenerator;

internal static class CodeDomExtensions
{
    public static IEnumerable<CodeAttributeDeclaration> GetSecurityAttributes(this ICustomAttributeProvider source, IEnumerable<Type> securityRuleTypeList)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var viewAttr = source.GetViewDomainObjectAttribute();

        if (viewAttr != null)
        {
            var securityRuleType = securityRuleTypeList.First(t =>
                                                              {

                                                                  var values =
                                                                      new[]
                                                                      {
                                                                          t.GetStaticPropertyValueList<SecurityOperation>().Select(v => (SecurityRule)v),
                                                                          t.GetStaticPropertyValueList<SecurityRole>().Select(v => (SecurityRule)v),
                                                                          t.GetStaticPropertyValueList<SecurityRule>()
                                                                      }.SelectMany();

                                                                  return values.Contains(viewAttr.SecurityRule);
                                                              });

            yield return viewAttr.GetType().ToTypeReference().ToAttributeDeclaration(

                viewAttr.GetViewSecurityAttributesArguments(securityRuleType).ToArray());
        }
    }

    private static IEnumerable<CodeAttributeArgument> GetViewSecurityAttributesArguments(this ViewDomainObjectAttribute viewAttr, Type securityRuleType)
    {
        yield return new CodeAttributeArgument { Value = securityRuleType.ToTypeOfExpression() };

        foreach (var operation in viewAttr.AllRules)
        {
            yield return new CodeAttributeArgument { Value = operation.ToString().ToPrimitiveExpression() };
        }
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this CustomSerializationAttribute attr)
    {
        if (attr == null) throw new ArgumentNullException(nameof(attr));

        if (!attr.Role.HasFlag(DTORole.Client))
        {
            throw new ArgumentException("Invalid attribute", nameof(attr));
        }

        var arg = new CodeAttributeArgument { Value = attr.Mode.ToPrimitiveExpression() };

        return typeof(CustomSerializationAttribute).ToTypeReference().ToAttributeDeclaration(arg);
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this ExpandPathAttribute expandPathAttr)
    {
        if (expandPathAttr == null) throw new ArgumentNullException(nameof(expandPathAttr));

        var arg = new CodeAttributeArgument { Value = expandPathAttr.Path.ToPrimitiveExpression() };

        return typeof(ExpandPathAttribute).ToTypeReference().ToAttributeDeclaration(arg);
    }



    public static CodeAttributeDeclaration ToAttributeDeclaration(this DependencySecurityAttribute dependencySecurityAttr)
    {
        if (dependencySecurityAttr == null) throw new ArgumentNullException(nameof(dependencySecurityAttr));

        return typeof(DependencySecurityAttribute).ToTypeReference().ToAttributeDeclaration(dependencySecurityAttr.GetArguments().ToArray());
    }

    private static IEnumerable<CodeAttributeArgument> GetArguments(this DependencySecurityAttribute dependencySecurityAttr)
    {
        if (dependencySecurityAttr == null) throw new ArgumentNullException(nameof(dependencySecurityAttr));

        yield return new CodeAttributeArgument { Value = dependencySecurityAttr.SourceType.ToTypeOfExpression() };

        if (!dependencySecurityAttr.IsUntyped)
        {
            yield return new CodeAttributeArgument { Name = nameof(DependencySecurityAttribute.Path), Value = dependencySecurityAttr.Path.ToDynamicPrimitiveExpression() };
        }
    }


    public static CodeAttributeDeclaration ToAttributeDeclaration(this BLLProjectionViewRoleAttribute projectionAttr)
    {
        if (projectionAttr == null) throw new ArgumentNullException(nameof(projectionAttr));

        return typeof(BLLProjectionViewRoleAttribute).ToTypeReference().ToAttributeDeclaration(projectionAttr.GetArguments().ToArray());
    }

    private static IEnumerable<CodeAttributeArgument> GetArguments(this BLLProjectionViewRoleAttribute projectionAttr)
    {
        if (projectionAttr == null) throw new ArgumentNullException(nameof(projectionAttr));

        if (projectionAttr.MaxFetch != ViewDTOType.ProjectionDTO)
        {
            yield return new CodeAttributeArgument { Name = nameof(BLLProjectionViewRoleAttribute.MaxFetch), Value = projectionAttr.MaxFetch.ToDynamicPrimitiveExpression() };
        }
    }


    public static CodeAttributeDeclaration ToAttributeDeclaration(this ProjectionAttribute projectionAttr)
    {
        if (projectionAttr == null) throw new ArgumentNullException(nameof(projectionAttr));

        return typeof(ProjectionAttribute).ToTypeReference().ToAttributeDeclaration(projectionAttr.GetArguments().ToArray());
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this ProjectionPropertyAttribute projectionAttr)
    {
        if (projectionAttr == null) throw new ArgumentNullException(nameof(projectionAttr));

        var arg = new CodeAttributeArgument { Value = projectionAttr.Role.ToPrimitiveExpression() };

        return typeof(ProjectionPropertyAttribute).ToTypeReference().ToAttributeDeclaration(arg);
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this ProjectionFilterAttribute projectionFilterAttr)
    {
        if (projectionFilterAttr == null) throw new ArgumentNullException(nameof(projectionFilterAttr));

        var arg1 = new CodeAttributeArgument { Value = new CodeTypeOfExpression(projectionFilterAttr.FilterType) };

        var arg2 = new CodeAttributeArgument { Value = projectionFilterAttr.Target.ToPrimitiveExpression() };

        return typeof(ProjectionFilterAttribute).ToTypeReference().ToAttributeDeclaration(arg1, arg2);
    }

    private static IEnumerable<CodeAttributeArgument> GetArguments(this ProjectionAttribute projectionAttr)
    {
        if (projectionAttr == null) throw new ArgumentNullException(nameof(projectionAttr));

        yield return new CodeAttributeArgument { Value = new CodeTypeOfExpression(projectionAttr.SourceType) };

        yield return new CodeAttributeArgument { Value = projectionAttr.Role.ToPrimitiveExpression() };

        if (projectionAttr.ContractType != null)
        {
            yield return new CodeAttributeArgument { Value = new CodeTypeOfExpression(projectionAttr.ContractType) };
        }
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this TableAttribute tableAttr)
    {
        if (tableAttr == null) throw new ArgumentNullException(nameof(tableAttr));

        return typeof(TableAttribute).ToTypeReference().ToAttributeDeclaration(tableAttr.GetArguments().ToArray());
    }

    private static IEnumerable<CodeAttributeArgument> GetArguments(this TableAttribute tableAttr)
    {
        if (tableAttr == null) throw new ArgumentNullException(nameof(tableAttr));

        if (!tableAttr.Name.IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(TableAttribute.Name), Value = tableAttr.Name.ToPrimitiveExpression() };
        }

        if (!tableAttr.Schema.IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(TableAttribute.Schema), Value = tableAttr.Schema.ToPrimitiveExpression() };
        }
    }


    public static CodeAttributeDeclaration ToAttributeDeclaration(this MappingAttribute mappingAttr)
    {
        if (mappingAttr == null) throw new ArgumentNullException(nameof(mappingAttr));

        return typeof(MappingAttribute).ToTypeReference().ToAttributeDeclaration(mappingAttr.GetArguments().ToArray());
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this MappingPropertyAttribute attr)
    {
        if (attr == null) throw new ArgumentNullException(nameof(attr));

        var canInsertArrb = new CodeAttributeArgument
        { Name = "CanInsert", Value = attr.CanInsert.ToPrimitiveExpression() };
        var canUpdateArrb = new CodeAttributeArgument
        { Name = "CanUpdate", Value = attr.CanUpdate.ToPrimitiveExpression() };

        return typeof(MappingPropertyAttribute).ToTypeReference().ToAttributeDeclaration(new[] { canInsertArrb, canUpdateArrb });
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this InlineBaseTypeMappingAttribute mappingAttr)
    {
        if (mappingAttr == null) throw new ArgumentNullException(nameof(mappingAttr));

        return typeof(InlineBaseTypeMappingAttribute).ToTypeReference().ToAttributeDeclaration();
    }

    private static IEnumerable<CodeAttributeArgument> GetArguments(this MappingAttribute mappingAttr)
    {
        if (mappingAttr == null) throw new ArgumentNullException(nameof(mappingAttr));

        if (!mappingAttr.ColumnName.IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(MappingAttribute.ColumnName), Value = mappingAttr.ColumnName.ToPrimitiveExpression() };
        }

        if (!mappingAttr.IsOneToOne.IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(MappingAttribute.IsOneToOne), Value = mappingAttr.IsOneToOne.ToPrimitiveExpression() };
        }

        if (!mappingAttr.GetActualCascadeMode().IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(MappingAttribute.CascadeMode), Value = mappingAttr.CascadeMode.ToPrimitiveExpression() };
        }

        if (!mappingAttr.ExternalTableName.IsDefault())
        {
            yield return new CodeAttributeArgument { Name = nameof(MappingAttribute.ExternalTableName), Value = mappingAttr.ExternalTableName.ToPrimitiveExpression() };
        }
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this IgnoreFetchAttribute attr)
    {
        if (attr == null) throw new ArgumentNullException(nameof(attr));

        return typeof(IgnoreFetchAttribute).ToTypeReference().ToAttributeDeclaration();
    }

    public static CodeAttributeDeclaration ToAttributeDeclaration(this FetchPathAttribute attr)
    {
        if (attr == null) throw new ArgumentNullException(nameof(attr));

        var arg = new CodeAttributeArgument { Value = attr.Path.ToPrimitiveExpression() };

        return typeof(FetchPathAttribute).ToTypeReference().ToAttributeDeclaration(arg);
    }
}
