using Framework.Core;

namespace Framework.DomainDriven.BLLCoreGenerator;

public static class TypeExtensions
{
    internal static string ToGetSecurityPathMethodName(this Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return $"Get{domainType.Name}SecurityPath";
    }

    internal static string ToGetSecurityProviderMethodName(this Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return $"Get{domainType.Name}SecurityProvider";
    }

    public static DirectMode GetDirectMode(this Type model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        return model.GetCustomAttribute<DirectModeAttribute>()
                    .FromMaybe(() => new Exception($"{typeof(DirectModeAttribute).Name} not found for type {model.Name}"))
                    .DirectMode;
    }

    public static void CheckDirectMode(this Type model, DirectMode directMode, bool allowEmptyAttr)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var modelDirectModeM = allowEmptyAttr ? model.GetCustomAttribute<DirectModeAttribute>().ToMaybe().Select(v => v.DirectMode) : Maybe.Return(model.GetDirectMode());

        modelDirectModeM.Match(modelDirectMode =>
                               {
                                   if (modelDirectMode != directMode)
                                   {
                                       throw new Exception($"DirectMode \"{directMode}\" not allowed for type \"{model}\". Expected: {directMode}");
                                   }
                               });
    }

    public static string GetModelMethodName(this Type domainType, Type model, ModelRole modelRole, bool withDomainBody, string modelRoleCustomPrefix = null)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));


        var bodyName = model.Name.Skip(domainType.Name, true).SkipLast(modelRole + "Model", true);

        var body = bodyName == "" ? "" : "By" + bodyName.Skip("By");

        return (modelRoleCustomPrefix ?? modelRole.ToString()) + (withDomainBody ? domainType.Name : "") + body;
    }

    internal static Dictionary<Type, Type> GetInterfaceImplementationArgumentDict(this Type type, Type interfaceType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        return interfaceType.GetGenericArguments().ZipStrong(
                                                             type.GetInterfaceImplementationArguments(interfaceType),
                                                             (genericArg, implArg) => genericArg.ToKeyValuePair(implArg)).ToDictionary();
    }
}
