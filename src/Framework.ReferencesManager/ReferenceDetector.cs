using System.Reflection;
using Framework.Persistent;

namespace Framework.ReferencesManager;

public struct PropertyDescription
{
    public Type Type { get; private set; }
    public string Name { get; private set; }
    public PropertyDescription(Type type, string name) : this()
    {
        this.Type = type;
        this.Name = name;
    }
}
public class ReferenceDetector : IReferenceDetector
{
    public IList<Reference> Find(Type objectType, string assemblyName)
    {
        var result = new List<Reference>();
        var detailObjectTypeCollection = this.GetDetailsTypeCollection(objectType);
        var assembly = Assembly.Load(assemblyName);
        foreach (var type in assembly.GetTypes())
        {
            var currentType = type;
            if (currentType.IsAbstract)
            {
                continue;
            }
            if (currentType.Equals(objectType))
            {
                continue;
            }
            if (currentType.GetCustomAttributes(typeof(IgnoreReference), true).Length > 0)
            {
                continue;
            }

            if (detailObjectTypeCollection.Any(@object => @object.Type.IsAssignableFrom(currentType)))
            {
                continue;
            }
            //if(detailObjectTypeCollection.Contains(currentType))
            //{
            //    continue;
            //}
            var ignoreTypes = new HashSet<Type>(new[]{typeof(IgnoreReference), typeof(ExpandPathAttribute)});
            foreach (var currentPropertyInfo in currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var isIgnore = null != currentPropertyInfo.GetCustomAttributes(true).FirstOrDefault(z=>ignoreTypes.Contains(z.GetType()));

                if (isIgnore)
                {
                    continue;
                }
                if (currentPropertyInfo.PropertyType.Equals(objectType))
                {
                    result.Add(new Reference(currentType, currentPropertyInfo.Name));
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Предоставляет коллекцию типов, содержащихся в требуемом типе в свойствах-листах.
    /// </summary>
    /// <param name="masterType"></param>
    /// <returns></returns>
    public List<PropertyDescription> GetDetailsTypeCollection(Type masterType)
    {
        var result = new List<PropertyDescription>();
        foreach (var currentPropertyInfo in masterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (currentPropertyInfo.GetCustomAttributes<DetailRoleAttribute>().Any(q => q.Role == DetailRole.No))
            {
                continue;
            }


            if (!currentPropertyInfo.PropertyType.IsGenericType)
            {
                if (!currentPropertyInfo.PropertyType.IsPrimitive)
                {
                    result.Add(new PropertyDescription(currentPropertyInfo.PropertyType, currentPropertyInfo.Name));
                }
            }
            else
            {
                var genericArguments = currentPropertyInfo.PropertyType.GetGenericArguments();

                if (genericArguments.Length > 1)
                {
                    continue;
                }
                var currentPropertyDescription = new PropertyDescription(genericArguments[0],
                                                                         currentPropertyInfo.Name);
                if (result.Contains(currentPropertyDescription))
                {
                    continue;
                }
                result.Add(currentPropertyDescription);
            }
        }
        return result;
    }
}
