using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Projection;

namespace Framework.DomainDriven.ProjectionGenerator;

public class ProjectionFileFactory<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly bool isPersistent;

    private readonly Type sourceType;

    private readonly Type contractType;

    public ProjectionFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.sourceType = this.DomainType.GetProjectionSourceType();

        this.contractType = this.DomainType.GetProjectionContractType();

        this.isPersistent = this.Configuration.IsPersistentObject(this.DomainType);
    }


    public override FileType FileType { get; } = FileType.Projection;

    public override CodeTypeReference BaseReference => this.Configuration.Environment.HasCustomProjectionProperties(this.DomainType)
                                                               ? this.Configuration.GetCodeTypeReference(this.DomainType, FileType.CustomProjectionBase)
                                                               : this.DomainType.BaseType.ToTypeReference(); //this.Configuration.Environment.GetProjectionBaseType(this.DomainType).ToTypeReference();


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       TypeAttributes = TypeAttributes.Public,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        foreach (var @interface in this.DomainType.GetInterfaces().Except(this.DomainType.BaseType.GetInterfaces()))
        {
            yield return @interface.ToTypeReference();
        }
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        return this.Configuration.GetDomainTypeAttributeDeclarations(this.DomainType);
    }

    private IEnumerable<PropertyInfo> GetProperties(bool includeBase)
    {
        return this.Configuration.Environment.GetProjectionProperties(this.DomainType, includeBase, false);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        foreach (var property in this.GetProperties(false))
        {
            var propertyTypeRef = property.PropertyType.ToTypeReference();

            var attributes = this.Configuration.GetPropertyAttributeDeclarations(property).ToArray();

            if (this.Configuration.IsIdentityProperty(property))
            {
                var genProp = new CodeMemberProperty
                              {
                                      Name = property.Name,

                                      Type = propertyTypeRef,

                                      Attributes = MemberAttributes.Public | MemberAttributes.Override,

                                      GetStatements = { new CodeBaseReferenceExpression().ToPropertyReference(property).ToMethodReturnStatement() }
                              };

                genProp.CustomAttributes.AddRange(attributes);

                yield return genProp;
            }
            else if (property.HasAttribute<ExpandPathAttribute>())
            {
                var genProp = this.CreateExpandProperty(property, false);

                genProp.CustomAttributes.AddRange(attributes);

                yield return genProp;
            }
            else
            {
                var fieldType = property.PropertyType.IsCollection() ? typeof(ICollection<>).MakeGenericType(property.PropertyType.GetCollectionElementType()) : property.PropertyType;

                var genField = new CodeMemberField
                               {
                                       Name = property.Name.ToStartLowerCase(),

                                       Type = fieldType.ToTypeReference()
                               };

                var genProp = new CodeMemberProperty
                              {
                                      Name = property.Name,

                                      Type = propertyTypeRef,

                                      Attributes = MemberAttributes.Public,

                                      GetStatements = { new CodeThisReferenceExpression().ToFieldReference(genField).ToMethodReturnStatement() }
                              };

                genProp.CustomAttributes.AddRange(attributes);

                if (property.HasAttribute<MappingAttribute>(mappingAttr => mappingAttr.IsOneToOne) && this.Configuration.OneToOneSetter)
                {
                    genProp.SetStatements.Add(new CodePropertySetValueReferenceExpression().ToAssignStatement(new CodeThisReferenceExpression().ToFieldReference(genField)));
                }

                yield return genField;

                yield return genProp;
            }
        }

        foreach (var interfaceType in this.DomainType.GetInterfaces())
        {
            var reverseInterfaceMap = this.DomainType.GetInterfaceMapDictionary(interfaceType);

            foreach (var privateProperty in this.DomainType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (reverseInterfaceMap.ContainsValue(privateProperty.GetMethod))
                {
                    var genProp = this.CreateExpandProperty(privateProperty, true);

                    genProp.PrivateImplementationType = interfaceType.ToTypeReference();

                    yield return genProp;
                }
            }
        }
    }

    private CodeMemberProperty CreateExpandProperty(PropertyInfo property, bool withAttr)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var expandPathAttr = property.GetCustomAttribute<ExpandPathAttribute>();

        var propertyPath = property.GetExpandPath(false);

        var getExpr = propertyPath.Aggregate(

                                             new { Expression = (CodeExpression)new CodeThisReferenceExpression(), IsRef = false },

                                             (state, propNode) =>
                                             {
                                                 if (state.IsRef)
                                                 {
                                                     return new { Expression = (CodeExpression)state.Expression.ToMaybePropertyReference(propNode), IsRef = true };
                                                 }
                                                 else
                                                 {
                                                     return new { Expression = (CodeExpression)state.Expression.ToPropertyReference(propNode), IsRef = propNode.PropertyType.IsNullable() || propNode.PropertyType.IsClass };
                                                 }
                                             },
                                             state =>
                                             {
                                                 if (propertyPath.Count > 1 && property.PropertyType.IsCollection())
                                                 {
                                                     return typeof(CommonFramework.EnumerableExtensions)
                                                            .ToTypeReferenceExpression()
                                                            .ToMethodInvokeExpression("EmptyIfNull", state.Expression);
                                                 }
                                                 else
                                                 {
                                                     return state.Expression;
                                                 }
                                             });

        var prop = new CodeMemberProperty
                   {
                           Name = property.Name,

                           Type = property.PropertyType.ToTypeReference(),

                           Attributes = MemberAttributes.Public,

                           GetStatements = { getExpr.ToMethodReturnStatement() },
                   };

        if (withAttr)
        {
            prop.CustomAttributes.Add(expandPathAttr.ToAttributeDeclaration());
        }

        return prop;
    }


    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        yield return new CodeConstructor
                     {
                             Attributes = this.Configuration.GeneratePublicCtors ? MemberAttributes.Public : MemberAttributes.Family
                     };

        if (this.contractType != null)
        {
            yield return this.GetSourceConstructor();
        }
    }

    private CodeConstructor GetSourceConstructor()
    {
        var parameter = this.contractType.ToTypeReference().ToParameterDeclarationExpression("source");
        var parameterVar = parameter.ToVariableReferenceExpression();

        return new CodeConstructor
               {
                       Attributes = MemberAttributes.Public,
                       Parameters = { parameter }
               }.WithStatements(this.GetSourceConstructorStatements(parameterVar));
    }

    private IEnumerable<CodeStatement> GetSourceConstructorStatements(CodeExpression sourceExpr)
    {
        var targetExpr = new CodeThisReferenceExpression();

        var baseProperties = this.contractType.GetAllInterfaceProperties();

        var basePropertiesDict = baseProperties.ToDictionary(p => p.Name);

        foreach (var targetProp in this.GetProperties(true))
        {
            var canSet = targetProp.ReflectedType == this.DomainType || targetProp.CanWrite;

            if (canSet && !targetProp.HasAttribute<ExpandPathAttribute>())
            {
                var sourceProp = basePropertiesDict[targetProp.Name];

                var sourcePropExpr = sourceExpr.ToPropertyReference(sourceProp);


                var targetMemberExpr = targetProp.ReflectedType == this.DomainType ? (CodeExpression)targetExpr.ToFieldReference(targetProp.Name.ToStartLowerCase())
                                               : targetExpr.ToPropertyReference(targetProp);

                if (sourceProp.PropertyType != targetProp.PropertyType)
                {
                    if (sourceProp.PropertyType.IsCollection())
                    {
                        var elementType = targetProp.PropertyType.GetCollectionElementType();

                        var lambda = new CodeParameterDeclarationExpression { Name = "v" }.Pipe(param => new CodeLambdaExpression
                            {
                                    Parameters = { param },

                                    Statements = { elementType.ToTypeReference().ToObjectCreateExpression(param.ToVariableReferenceExpression()).ToMethodReturnStatement() }
                            });

                        yield return typeof(CoreEnumerableExtensions)
                                     .ToTypeReferenceExpression()
                                     .ToMethodInvokeExpression("ToList", sourcePropExpr, lambda)
                                     .ToAssignStatement(targetMemberExpr);
                    }
                    else
                    {
                        yield return new CodeNotNullConditionStatement(sourcePropExpr)
                                     {
                                             TrueStatements =
                                             {
                                                     targetProp.PropertyType.ToTypeReference().ToObjectCreateExpression(sourcePropExpr).ToAssignStatement(targetMemberExpr)
                                             }
                                     };
                    }
                }
                else
                {
                    yield return sourcePropExpr.ToAssignStatement(targetMemberExpr);
                }
            }
        }
    }
}
