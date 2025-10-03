using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Security;
using Framework.Validation;

namespace Framework.Projection.Contract;

internal class GeneratedType : BaseTypeImpl
{
    private readonly ProjectionContractEnvironment environment;

    internal readonly Type ContractType;


    private readonly GeneratedProperty[] generatedProperties;

    private readonly IReadOnlyDictionary<GeneratedProperty, ExplicitProperty> explicitProperties;

    private readonly GeneratedField[] generatedFields;


    internal readonly Type SourceType;

    private readonly bool isPersistent;

    private readonly Attribute[] customAttributes;


    public GeneratedType(ProjectionContractEnvironment environment, Type contractType, Dictionary<Type, GeneratedType> preGenerateTypes)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));

        preGenerateTypes.Add(contractType, this);

        this.environment = environment;
        this.ContractType = contractType;

        this.Name = contractType.Name.Skip("I", true);

        this.SourceType = contractType.GetProjectionContractSourceType();

        this.isPersistent = this.environment.PersistentDomainObjectBaseType.IsAssignableFrom(this.SourceType);

        this.generatedProperties = this.GetGenerateProperties().ToArray();
        this.generatedFields = this.GetGenerateFields().ToArray();
        this.explicitProperties = this.GetExplicitProperties();

        this.customAttributes = this.GetCustomAttributes();
    }


    public override string FullName => $"{this.Namespace}.{this.Name}";

    public override string Name { get; }

    public override string Namespace => this.environment.Namespace;

    public override string AssemblyQualifiedName => $"{this.FullName}, {this.environment.Assembly.FullName}";

    public override Type UnderlyingSystemType => this.BaseType;

    public override Type BaseType => this.isPersistent ? this.environment.PersistentDomainObjectBaseType : this.environment.DomainObjectBaseType;

    public override Assembly Assembly { get; } = null;

    public override Module Module { get; } = typeof(GeneratedType).Module;

    public override Type[] GetInterfaces()
    {
        return this.ContractType.GetAllInterfaces().ToArray();
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

    private Attribute[] GetCustomAttributes()
    {
        return new []
               {
                       this.GetSourceTypeAttributes(),

                       this.ContractType.GetCustomAttributes().Where(v => !(v is ProjectionContractAttribute)),

                       this.GetTableAttributes(),

                       this.GetProjectionAttributes(),

                       this.GetSecurityAttributes()

               }.SelectMany().ToArray();
    }

    private IEnumerable<Attribute> GetSourceTypeAttributes()
    {
        return this.SourceType.GetCustomAttributes().Where(attr =>
                                                                   !(attr is TableAttribute)
                                                                   && !(attr is BLLRoleAttribute)
                                                                   && !(attr is ClassValidatorAttribute)
                                                                   && !(attr is DomainObjectAccessAttribute)
                                                                   && !(attr is DependencySecurityAttribute));
    }

    private IEnumerable<Attribute> GetSecurityAttributes()
    {
        if (this.isPersistent)
        {
            yield return new DependencySecurityAttribute(this.SourceType);
        }
    }

    private IEnumerable<TableAttribute> GetTableAttributes()
    {
        yield return this.SourceType.GetCustomAttribute<TableAttribute>() ?? new TableAttribute { Name = this.SourceType.Name };
    }

    private IEnumerable<ProjectionAttribute> GetProjectionAttributes()
    {
        yield return new ProjectionAttribute(this.SourceType, ProjectionRole.Default, this.ContractType);
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
        if (bindingAttr.HasFlag(BindingFlags.Instance | BindingFlags.NonPublic))
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
            }

            if (bindingAttr.HasFlag(BindingFlags.NonPublic))
            {
                yield return this.explicitProperties.Values;
            }
        }
    }

    protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
    {
        return this.generatedProperties.SingleOrDefault(prop => prop.Name == name)

               ?? this.BaseType.GetProperty(name, bindingAttr);
    }

    public override InterfaceMapping GetInterfaceMap(Type interfaceType)
    {
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        var request = from prop in this.generatedProperties

                      where prop.ContractProperty.ReflectedType == interfaceType

                      let explicitProp = this.explicitProperties.GetValueOrDefault(prop)

                      let implementProp = explicitProp ?? (PropertyInfo)prop

                      select new
                             {
                                     InterfaceMethod = prop.ContractProperty.GetMethod,

                                     ImplementMethod = implementProp.GetMethod
                             };

        var methods = request.ToArray();

        return new InterfaceMapping
               {
                       InterfaceType = interfaceType,
                       TargetType = this,
                       InterfaceMethods = methods.ToArray(pair => pair.InterfaceMethod),
                       TargetMethods = methods.ToArray(pair => pair.ImplementMethod)
               };
    }

    public override bool Equals(Type o)
    {
        return ReferenceEquals(this, o);
    }

    public override int GetHashCode()
    {
        return this.FullName.GetHashCode();
    }

    private IEnumerable<GeneratedProperty> GetGenerateProperties()
    {
        var exceptInterfaces = this.BaseType.GetAllInterfaces();

        foreach (var contractProperty in this.ContractType.GetAllInterfaces().Except(exceptInterfaces).SelectMany(i => i.GetProperties()))
        {
            yield return new GeneratedProperty(this.environment, contractProperty, this);
        }
    }

    private IEnumerable<GeneratedField> GetGenerateFields()
    {
        foreach (var property in this.generatedProperties)
        {
            if (!property.HasAttribute<ExpandPathAttribute>())
            {
                yield return new GeneratedField(this.environment, property, this);
            }
        }
    }

    private IReadOnlyDictionary<GeneratedProperty, ExplicitProperty> GetExplicitProperties()
    {
        var request = from generateProp in this.generatedProperties

                      where generateProp.Name != generateProp.ContractProperty.Name || generateProp.PropertyType != generateProp.ContractProperty.PropertyType

                      select (generateProp, new ExplicitProperty(generateProp.ContractProperty, generateProp.ReflectedType, generateProp.Name, generateProp.ContractProperty.PropertyType));

        return request.ToDictionary();
    }
}
