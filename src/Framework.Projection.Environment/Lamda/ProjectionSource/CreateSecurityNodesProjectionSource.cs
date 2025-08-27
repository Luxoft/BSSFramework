using CommonFramework;
using Framework.Core;
using Framework.Persistent;
using Framework.Security;

namespace Framework.Projection.Lambda;

internal class CreateSecurityNodesProjectionSource : IProjectionSource
{
    private readonly IProjectionSource baseSource;

    private readonly ProjectionLambdaEnvironment environment;


    public CreateSecurityNodesProjectionSource(IProjectionSource baseSource, ProjectionLambdaEnvironment environment)
    {
        this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public IEnumerable<IProjection> GetProjections()
    {
        var builders = this.baseSource.GetProjections().ToBuilders();

        var allTypes = builders.SelectMany(projection => new[] { projection.SourceType }.Concat(projection.Properties.Select(prop => prop.ElementType)))
                               .Distinct()
                               .ToArray();

        var securityProjections = this.GetLinkedSecurityTypes(allTypes).SelectMany(sourceType => new[] { sourceType }.Concat(sourceType.GetHierarchicalSecurityTypes()))
                                      .ToDictionary(sourceType => sourceType, sourceType => new ProjectionBuilder(sourceType) { Name = $"Security{sourceType.Name}", Role = ProjectionRole.SecurityNode });

        foreach (var sourceType in securityProjections.Keys)
        {
            yield return this.FillSecurityProjection(sourceType, securityProjections);
        }

        foreach (var projection in builders)
        {
            yield return projection;
        }
    }

    private ProjectionBuilder FillSecurityProjection(Type sourceType, IReadOnlyDictionary<Type, ProjectionBuilder> securityProjections)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
        if (securityProjections == null) throw new ArgumentNullException(nameof(securityProjections));

        var projection = securityProjections[sourceType];

        var allSecurityInterfaces = sourceType.GetSecurityNodeInterfaces().Concat(sourceType.GetExtraSecurityNodeInterface().MaybeYield())
                                              .SelectMany(i => i.GetAllInterfaces())
                                              .Distinct()
                                              .Except(this.environment.PersistentDomainObjectBaseType.GetAllInterfaces())
                                              .ToList();

        foreach (var interfaceType in allSecurityInterfaces)
        {
            foreach (var interfaceProp in interfaceType.GetProperties())
            {
                var implProp = sourceType.GetImplementedProperty(interfaceProp);

                var isExplicit = implProp.GetExpandPath().Maybe(path => path.IsEmpty);

                var name = $"{interfaceProp.Name}_Security";

                projection.Properties.Add(new ProjectionPropertyBuilder(implProp.ToGetLambdaExpression(sourceType))
                                          {
                                                  Role = ProjectionPropertyRole.Security,
                                                  Name = name,
                                                  IgnoreSerialization = true,
                                                  ElementProjection = securityProjections.GetValueOrDefault(implProp.PropertyType.GetCollectionOrArrayElementTypeOrSelf()),
                                                  VirtualExplicitInterfaceProperty = isExplicit ? interfaceProp : null
                                          });
            }
        }

        return projection;
    }

    private HashSet<Type> GetLinkedSecurityTypes(IEnumerable<Type> allTypes)
    {
        if (allTypes == null) throw new ArgumentNullException(nameof(allTypes));

        var securityTypes = new HashSet<Type>();

        this.FillLinkedSecurityTypes(allTypes, securityTypes);

        return securityTypes;
    }

    private void FillLinkedSecurityTypes(IEnumerable<Type> allTypes, HashSet<Type> securityTypes)
    {
        if (allTypes == null) throw new ArgumentNullException(nameof(allTypes));
        if (securityTypes == null) throw new ArgumentNullException(nameof(securityTypes));

        var request = from type in allTypes

                      from securityType in this.GetSecurityNodeArgsWithSelf(type)

                      select securityType;

        foreach (var securityType in request)
        {
            if (securityTypes.Add(securityType))
            {
                this.FillLinkedSecurityTypes(new[] { securityType }, securityTypes);
            }
        }
    }

    private IEnumerable<Type> GetSecurityNodeArgsWithSelf(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var securityNodeInterfaces = type.GetSecurityNodeInterfaces().ToArray();

        if (securityNodeInterfaces.Any())
        {
            var request = from genSecurityType in type.GetSecurityNodeInterfaces()

                          from securityType in genSecurityType.GetGenericArguments()

                          select securityType;


            return request.Concat(new[] { type });
        }
        else
        {
            return Type.EmptyTypes;
        }
    }
}
