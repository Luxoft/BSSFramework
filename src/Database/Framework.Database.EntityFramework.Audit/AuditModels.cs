using System.Reflection;
using System.Reflection.Emit;

using Framework.Database.Domain;

namespace Framework.Database.EntityFramework.Audit;

public sealed class AuditEntityFactory : IAuditEntityFactory
{
    public string RevisionIdPropertyName => "RevisionId";
    public string RevisionPropertyName => "Revision";
    public string RevisionTypePropertyName => "RevisionType";

    private readonly ModuleBuilder moduleBuilder = AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName("EF.Audit.DynamicAudits"), AssemblyBuilderAccess.Run)
        .DefineDynamicModule("DynamicAudits");

    private readonly Dictionary<Type, AuditEntityMetadata> metadataByEntityType = [];

    public AuditEntityMetadata GetOrCreate(
        Type entityType,
        IEnumerable<AuditPropertyMetadata> properties)
    {
        lock (this.metadataByEntityType)
        {
            if (this.metadataByEntityType.TryGetValue(entityType, out var metadata))
            {
                return metadata;
            }

            var auditProperties = properties.ToArray();
            var typeBuilder = this.moduleBuilder.DefineType(
                $"{entityType.Namespace}.{entityType.Name}Audit",
                TypeAttributes.Public | TypeAttributes.Class);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            foreach (var property in auditProperties)
            {
                this.DefineAutoProperty(typeBuilder, property.Name, property.PropertyType);
                if (!property.IsKey)
                {
                    this.DefineAutoProperty(typeBuilder, $"{property.Name}_MOD", typeof(bool));
                }
            }

            this.DefineAutoProperty(typeBuilder, this.RevisionIdPropertyName, typeof(long));
            this.DefineAutoProperty(typeBuilder, this.RevisionPropertyName, typeof(AuditRevisionEntity));
            this.DefineAutoProperty(typeBuilder, this.RevisionTypePropertyName, typeof(AuditRevisionType));

            metadata = new AuditEntityMetadata(entityType, typeBuilder.CreateType()!, auditProperties);
            this.metadataByEntityType.Add(entityType, metadata);
            return metadata;
        }
    }

    public bool TryGet(Type entityType, out AuditEntityMetadata metadata)
    {
        lock (this.metadataByEntityType)
        {
            return this.metadataByEntityType.TryGetValue(entityType, out metadata!);
        }
    }

    private void DefineAutoProperty(TypeBuilder typeBuilder, string name, Type propertyType)
    {
        var field = typeBuilder.DefineField($"_{name}", propertyType, FieldAttributes.Private);
        var property = typeBuilder.DefineProperty(name, PropertyAttributes.None, propertyType, null);
        var getter = typeBuilder.DefineMethod(
            $"get_{name}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            propertyType,
            Type.EmptyTypes);
        var getterIl = getter.GetILGenerator();
        getterIl.Emit(OpCodes.Ldarg_0);
        getterIl.Emit(OpCodes.Ldfld, field);
        getterIl.Emit(OpCodes.Ret);

        var setter = typeBuilder.DefineMethod(
            $"set_{name}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            null,
            [propertyType]);
        var setterIl = setter.GetILGenerator();
        setterIl.Emit(OpCodes.Ldarg_0);
        setterIl.Emit(OpCodes.Ldarg_1);
        setterIl.Emit(OpCodes.Stfld, field);
        setterIl.Emit(OpCodes.Ret);

        property.SetGetMethod(getter);
        property.SetSetMethod(setter);
    }
}
