using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Security;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal class GeneratedType : BaseTypeImpl
{
    private readonly ProjectionLambdaEnvironment environment;

    internal readonly IProjection Projection;


    private readonly GeneratedProperty[] generatedProperties;

    private readonly ExplicitProperty[] explicitProperties;

    private readonly GeneratedField[] generatedFields;

    private readonly GeneratedCustomProperty[] generatedCustomProperties;


    internal readonly Type SourceType;

    private readonly bool isPersistent;

    private readonly IReadOnlyCollection<Type> securityNodeInterfaces;

    private readonly Attribute[] customAttributes;


    public GeneratedType(ProjectionLambdaEnvironment environment, [NotNull] IProjection projection, Dictionary<IProjection, GeneratedType> preGenerateTypes)
    {
        if (projection == null) throw new ArgumentNullException(nameof(projection));

        preGenerateTypes.Add(projection, this);

        this.environment = environment;
        this.Projection = projection;

        this.Name = projection.Name;

        this.SourceType = projection.SourceType;

        this.isPersistent = this.environment.PersistentDomainObjectBaseType.IsAssignableFrom(this.SourceType);

        this.securityNodeInterfaces = this.GetSecurityNodeImplementInterfaces().Concat(this.GetExtraSecurityRoleInterface().MaybeYield()).ToArray();

        this.generatedProperties = this.GetGeneratedProperties().ToArray();
        this.generatedFields = this.GetGeneratedFields().ToArray();
        this.explicitProperties = this.GetExplicitProperties().Concat(this.GetSecurityExplicitProperties()).ToArray();
        this.generatedCustomProperties = this.GetGeneratedCustomProperties().ToArray();

        this.customAttributes = this.Projection.Attributes.ToArray();
    }


    public override string FullName => $"{this.Namespace}.{this.Name}";

    public override string Name { get; }

    public override string Namespace => this.environment.Namespace;

    public override string AssemblyQualifiedName => $"{this.FullName}, {this.environment.Assembly.FullName}";

    public override Type UnderlyingSystemType => this.isPersistent ? this.environment.PersistentDomainObjectBaseType : this.environment.DomainObjectBaseType;

    /// <summary>
    /// Флаг указания, что проекция наследуется от базовой секурной проекции.
    /// </summary>
    private bool HasBaseSecurityType => this.Projection.Role == ProjectionRole.Default
                                        && !this.environment.UseDependencySecurity
                                        && this.Projection.SourceType.HasSecurityNodeInterfaces();

    [NotNull]
    public override Type BaseType =>

            this.isPersistent ? this.HasBaseSecurityType ? this.environment.GetSecurityProjectionType(this.SourceType) : this.environment.PersistentDomainObjectBaseType
                    : this.environment.DomainObjectBaseType;

    public override Assembly Assembly { get; } = null;

    public override Module Module { get; } = typeof(GeneratedType).Module;

    public override Type[] GetInterfaces()
    {
        var currentTypeSecurityInterfaces = this.GetCurrentTypeSecurityInternalInterfaces().SelectMany().Distinct().ToArray();

        return (currentTypeSecurityInterfaces.Concat(this.BaseType.GetInterfaces())).Distinct().ToArray();
    }

    private IEnumerable<IEnumerable<Type>> GetCurrentTypeSecurityInternalInterfaces()
    {
        foreach (var securityNodeInterface in this.securityNodeInterfaces)
        {
            if (securityNodeInterface.IsGenericType)
            {
                var genericDefinition = securityNodeInterface.GetGenericTypeDefinition();

                var genericArgs = genericDefinition.GetGenericArguments();

                var implementedArgs = securityNodeInterface.GetGenericArguments();

                var argDict = genericArgs.ZipStrong(implementedArgs, (genArg, implArg) => genArg.ToKeyValuePair(implArg)).ToDictionary();

                var wrappedSubInterfaces = genericDefinition.GetAllInterfaces(false).ToArray();

                var implementedSubInterfaces = wrappedSubInterfaces.ToArray(wrappedSubInterface =>

                                                                                    wrappedSubInterface.IsGenericType ? wrappedSubInterface.GetGenericTypeDefinition().CachedMakeGenericType(wrappedSubInterface.GetGenericArguments().ToArray(wrappedArg => argDict[wrappedArg]))
                                                                                            : wrappedSubInterface);

                yield return implementedSubInterfaces;
            }
            else
            {
                yield return new[] { securityNodeInterface };
            }
        };
    }

    private Type GetExtraSecurityRoleInterface()
    {
        if (!this.environment.UseDependencySecurity && this.Projection.Role == ProjectionRole.SecurityNode)
        {
            var extraSecurityNodeInterfaceType = this.SourceType.GetExtraSecurityNodeInterface();

            if (extraSecurityNodeInterfaceType != null)
            {
                var baseDefinition = extraSecurityNodeInterfaceType.GetGenericTypeDefinition();

                var args = this.SourceType.GetInterfaceImplementationArguments(baseDefinition);

                var projectionArgs = args.ToArray(arg => this.environment.IsPersistent(arg) ? this.environment.GetProjectionTypeByRole(arg, ProjectionRole.SecurityNode) : arg);

                var projectionDefinition = baseDefinition.CachedMakeGenericType(projectionArgs);

                return projectionDefinition;
            }
        }

        return null;
    }

    private IEnumerable<Type> GetSecurityNodeImplementInterfaces()
    {
        if (this.Projection.Role == ProjectionRole.SecurityNode)
        {
            foreach (var securityInterface in this.SourceType.GetSecurityNodeInterfaces())
            {
                var securityArgs = securityInterface.GetGenericArguments().ToArray(this.environment.GetSecurityProjectionType);

                var projectionSecurityInterface = securityInterface.IsGenericType
                                                          ? securityInterface.GetGenericTypeDefinition().CachedMakeGenericType(securityArgs)
                                                          : securityInterface;

                yield return projectionSecurityInterface;
            }
        }
    }

    protected override bool IsArrayImpl()
    {
        return false;
    }

    protected override bool HasElementTypeImpl()
    {
        return false;
    }

    protected override bool IsPointerImpl()
    {
        return false;
    }

    protected override bool IsByRefImpl()
    {
        return false;
    }

    public override bool IsAssignableFrom(Type c)
    {
        return this.Equals(c);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return (object[])this.customAttributes.Where(attributeType.IsInstanceOfType).ToArray(attributeType);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        return this.customAttributes;
    }

    protected override TypeAttributes GetAttributeFlagsImpl()
    {
        return TypeAttributes.BeforeFieldInit;
    }

    protected override bool IsPrimitiveImpl()
    {
        return false;
    }

    public override FieldInfo GetField(string name, BindingFlags bindingAttr)
    {
        return this.GetFields(bindingAttr).SingleOrDefault(f => f.Name == name);
    }

    public override FieldInfo[] GetFields(BindingFlags bindingAttr)
    {
        if (bindingAttr.HasFlag(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic))
        {
            return this.generatedFields;
        }
        else
        {
            return new FieldInfo[0];
        }
    }

    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
        return this.GetInternalProperties(bindingAttr).SelectMany().ToArray();
    }

    private IEnumerable<IEnumerable<PropertyInfo>> GetInternalProperties(BindingFlags bindingAttr)
    {
        yield return this.BaseType.GetProperties(bindingAttr);

        if (bindingAttr.HasFlag(BindingFlags.Instance))
        {
            if (bindingAttr.HasFlag(BindingFlags.Public))
            {
                yield return this.generatedProperties;

                yield return this.generatedCustomProperties;
            }

            if (bindingAttr.HasFlag(BindingFlags.NonPublic))
            {
                yield return this.explicitProperties;
            }
        }
    }

    protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
    {
        return this.generatedProperties.SingleOrDefault(prop => prop.Name == name)

               ?? this.generatedCustomProperties.SingleOrDefault(prop => prop.Name == name)

               ?? this.BaseType.GetProperty(name, bindingAttr);
    }

    public override InterfaceMapping GetInterfaceMap([NotNull] Type interfaceType)
    {
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        var realInterface = interfaceType.IsGenericType ? interfaceType.GetGenericTypeDefinition() : interfaceType;

        var interfaceExplicitProperties = this.explicitProperties.Where(prop => prop.InterfaceProp.ReflectedType == realInterface).ToArray();

        return new InterfaceMapping
               {
                       InterfaceType = interfaceType,
                       TargetType = this,
                       InterfaceMethods = interfaceExplicitProperties.ToArray(prop => prop.InterfaceProp.GetMethod),
                       TargetMethods = interfaceExplicitProperties.ToArray(prop => prop.GetMethod)
               };
    }

    public override bool Equals(Type o)
    {
        return object.ReferenceEquals(this, o);
    }

    public override int GetHashCode()
    {
        return this.FullName.GetHashCode();
    }

    private IEnumerable<GeneratedProperty> GetGeneratedProperties()
    {
        foreach (var projectionProperty in this.Projection.Properties)
        {
            if (projectionProperty.VirtualExplicitInterfaceProperty == null)
            {
                yield return new GeneratedProperty(this.environment, projectionProperty, this);
            }
        }
    }


    private IEnumerable<GeneratedCustomProperty> GetGeneratedCustomProperties()
    {
        foreach (var projectionProperty in this.Projection.CustomProperties)
        {
            yield return new GeneratedCustomProperty(this.environment, projectionProperty, this);
        }
    }

    private IEnumerable<GeneratedField> GetGeneratedFields()
    {
        foreach (var property in this.generatedProperties)
        {
            if (!property.HasAttribute<ExpandPathAttribute>() && !property.IsIdentity)
            {
                yield return new GeneratedField(this.environment, property, this);
            }
        }
    }

    private IEnumerable<ExplicitProperty> GetExplicitProperties()
    {
        var allInterfaceProperties = this.securityNodeInterfaces.SelectMany(genericSecurityInterface =>
                                                                                    (genericSecurityInterface.IsGenericType ? genericSecurityInterface.GetGenericTypeDefinition() : genericSecurityInterface).GetAllInterfaceProperties()).Distinct().ToArray();

        var request = from generateProp in this.generatedProperties

                      join interfaceProp in allInterfaceProperties on generateProp.Name equals $"{interfaceProp.Name}_Security"

                      select new ExplicitProperty(interfaceProp, this, generateProp.Name, generateProp.PropertyType);

        foreach (var explicitProperty in request)
        {
            yield return explicitProperty;
        }
    }

    private IEnumerable<ExplicitProperty> GetSecurityExplicitProperties()
    {
        foreach (var projectionProperty in this.Projection.Properties)
        {
            if (projectionProperty.VirtualExplicitInterfaceProperty != null)
            {
                var propType = this.environment.BuildPropertyType(projectionProperty.Type);

                yield return new ExplicitProperty(projectionProperty.VirtualExplicitInterfaceProperty, this, projectionProperty.VirtualExplicitInterfaceProperty.Name, propType, projectionProperty.Path);
            }
        }
    }
}
