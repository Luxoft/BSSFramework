using System.Runtime.CompilerServices;
using System.Reflection;
using Framework.Persistent.Mapping;
using Framework.Core;

namespace Framework.DomainDriven.Metadata;

internal static class Extensions
{
    internal static IList<FieldInfo> GetCurrentAndUpAbstractTypeFields(this Type type)
    {
        var allTypes = new[] { type }
                       .Concat(type.BaseType.Maybe(z => z.GetAllElements(q => q.BaseType)
                                                         .TakeWhile(q => null != q && q != typeof(object) && q.IsAbstract)))
                       .ToList();

        var result =
                allTypes.SelectMany(
                                    z =>
                                            z.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                                                        BindingFlags.NonPublic)).ToList();

        return result;
    }
}

public static class MetadataReader
{
    private static readonly Func<FieldInfo, DomainTypeMetadata, FieldMetadata>
            listTypeFieldMetadataCreator
                    = (f, d) =>
                      {
                          var isCompilerGenerated = false;
                          var fieldName = f.Name;
                          if (f.IsDefined(typeof(CompilerGeneratedAttribute), false))
                          {
                              isCompilerGenerated = true;
                              fieldName = d.DomainType
                                           .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                           .Where(z => (z.GetGetMethod(true) ?? z.GetSetMethod(true)).IsDefined(typeof(CompilerGeneratedAttribute), false))
                                           .Select(z => new { GeneratedName = $"<{z.Name}>k__BackingField", Property = z })
                                           .First(z => string.Equals(z.GeneratedName, f.Name, StringComparison.InvariantCultureIgnoreCase))
                                           .Property.Name;
                          }
                          var result = f.FieldType.IsDomainTypeList(d.AssemblyMetadata.PersistentDomainObjectBaseType)
                                               ? new ListTypeFieldMetadata(d.DomainType, fieldName, isCompilerGenerated, f.FieldType, f.GetCollectionAttributes(d.DomainType), d) : null;
                          return result;
                      };

    private static readonly Func<FieldInfo, DomainTypeMetadata, FieldMetadata>
            inlineTypeFieldMetadataCreator
                    = (f, d) =>
                      {
                          InlineTypeFieldMetadata result;
                          if (f.FieldType.IsInlineType(d.AssemblyMetadata.PersistentDomainObjectBaseType))
                          {
                              result = new InlineTypeFieldMetadata(f.Name, f.FieldType, f.GetAttributes(d.DomainType), d);

                          }
                          else
                          {
                              result = null;
                          }

                          return result;
                      };

    private static readonly Func<FieldInfo, DomainTypeMetadata, FieldMetadata>
            referenceTypeFieldMetadataCreator
                    = (f, d) =>
                      {
                          if (f.GetCustomAttributes<NotPersistentFieldAttribute>().Any())
                          {
                              return null;
                          }
                          var result = f.FieldType.IsDomainType(d.AssemblyMetadata.PersistentDomainObjectBaseType)
                                               ? new ReferenceTypeFieldMetadata(f.Name, f.FieldType, f.GetAttributes(d.DomainType).Cast<Attribute>(), d) : null;
                          return result;
                      };

    private static readonly Func<FieldInfo, DomainTypeMetadata, FieldMetadata>
            primitiveTypeFieldMetadataCreator
                    = (f, d) =>
                      {
                          if (f.GetCustomAttributes<NotPersistentFieldAttribute>().Any())
                          {
                              return null;
                          }
                          if (f.FieldType.IsPrimitiveExt())
                          {
                              return new PrimitiveTypeFieldMetadata(f.Name, f.FieldType, f.GetAttributes(d.DomainType), d, f.Name.ToLower() == "id");
                          }
                          if (f.FieldType.IsArray && f.FieldType.GetElementType() == typeof(byte))
                          {
                              return new PrimitiveTypeFieldMetadata(f.Name, f.FieldType, f.GetAttributes(d.DomainType), d, false);
                          }
                          return null;
                      };

    private static IEnumerable<Func<FieldInfo, DomainTypeMetadata, FieldMetadata>> GetFieldConveyer()
    {
        yield return primitiveTypeFieldMetadataCreator;
        yield return referenceTypeFieldMetadataCreator;
        yield return listTypeFieldMetadataCreator;
        yield return inlineTypeFieldMetadataCreator;
    }

    static Func<FieldInfo, DomainTypeMetadata, FieldMetadata> fieldMetadataCreator = (f, d) =>
                                                                                     {
                                                                                         foreach (var func in GetFieldConveyer())
                                                                                         {
                                                                                             var fieldMetadata = func(f, d);
                                                                                             if (fieldMetadata != null)
                                                                                             {
                                                                                                 return fieldMetadata;
                                                                                             }
                                                                                         }

                                                                                         return null;
                                                                                     };

    static Func<Type, AssemblyMetadata, DomainTypeMetadata> domainTypeMetadataCreator4DeepFields = (t, a) =>
    {
        return domainTypeMetadataCreatorBase(t, a,
                                             t.GetCurrentAndUpAbstractTypeFields()
                                              .Where(z => !z.HasAttribute<NotPersistentFieldAttribute>()));
    };

    private static Func<Type, AssemblyMetadata, IEnumerable<FieldInfo>, DomainTypeMetadata> domainTypeMetadataCreatorBase =
            (type, assemblyMetadata, fieldInfoCollection) =>
            {
                var result = new DomainTypeMetadata(type, assemblyMetadata);

                var r = new List<FieldMetadata>();

                foreach (var fieldInfo in fieldInfoCollection)
                {
                    var z = fieldMetadataCreator(fieldInfo, result);
                    r.Add(z);
                }

                result.AddFields(r.Where(z => null != z));
                return result;
            };

    public static AssemblyMetadata GetAssemblyMetadata(Type persistentDomainObjectBase, params IAssemblyInfo[] assemblies)
    {
        var allTypes = assemblies.SelectMany(z => z.GetTypes()).ToArray();

        var result = new AssemblyMetadata(persistentDomainObjectBase);
        var typesToProcess = allTypes
                             .Where(z => !z.IsAbstract)
                             .Where(z => persistentDomainObjectBase.IsAssignableFrom(z))
                             .Where(z => z.BaseType.IsAbstract)
                             .Where(
                                    z => z.BaseType.GetAllElements(q => q.BaseType)
                                          .TakeWhile(q => q != null && q != persistentDomainObjectBase)
                                          .All(q => q.IsAbstract))
                             .Where(z => !z.HasAttribute<NotPersistentClassAttribute>());

        result.DomainTypes = typesToProcess
                             .Select(t => domainTypeMetadataCreator4DeepFields(t, result))
                             .Where(d => null != d).ToList();

        ProcessDomainTypes(allTypes, result.DomainTypes);
        return result;
    }

    private static void ProcessDomainTypes(Type[] allTypes, IEnumerable<DomainTypeMetadata> types)
    {
        var availableTypes = allTypes
                             .Where(z => !z.IsAbstract)
                             .Where(z => null != z.BaseType)
                             .ToList();

        foreach (var typeMetadata in types)
        {
            var subClasses = availableTypes
                             .Where(z => z.BaseType.GetAllElements(q => q.BaseType).Contains(typeMetadata.DomainType))
                             .Select(t => domainTypeMetadataCreator4DeepFields(t, typeMetadata.AssemblyMetadata))
                             .Where(d => null != d).ToList();

            foreach (var curentChild in subClasses)
            {
                typeMetadata.AddChildren(curentChild);
            }

            typeMetadata.EndDeclaration();

            ProcessDomainTypes(allTypes, subClasses);
        }
    }
}
