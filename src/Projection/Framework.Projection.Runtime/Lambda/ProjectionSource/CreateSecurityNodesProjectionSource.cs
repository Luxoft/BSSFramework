using CommonFramework;

using Framework.BLL.Domain.Extensions;
using Framework.Core;
using Framework.Projection.Lambda.Extensions;
using Framework.Projection.Lambda.ProjectionBuilder;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class CreateSecurityNodesProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var builders = baseSource.GetProjections().ToBuilders();

        var allTypes = builders.SelectMany(projection => new[] { projection.SourceType }.Concat(projection.Properties.Select(prop => prop.ElementType)))
                               .Distinct()
                               .ToArray();

        var securityProjections = this.GetLinkedSecurityTypes(allTypes)
                                      .ToDictionary(sourceType => sourceType, sourceType => new ProjectionBuilder.ProjectionBuilder(sourceType) { Name = $"Security{sourceType.Name}", Role = ProjectionRole.SecurityNode });

        foreach (var sourceType in securityProjections.Keys)
        {
            yield return this.FillSecurityProjection(sourceType, securityProjections);
        }

        foreach (var projection in builders)
        {
            yield return projection;
        }
    }

    private ProjectionBuilder.ProjectionBuilder FillSecurityProjection(Type sourceType, IReadOnlyDictionary<Type, ProjectionBuilder.ProjectionBuilder> securityProjections)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
        if (securityProjections == null) throw new ArgumentNullException(nameof(securityProjections));

        var projection = securityProjections[sourceType];

        var allSecurityInterfaces = sourceType.GetSecurityNodeInterfaces()
                                              .SelectMany(i => i.GetAllInterfaces())
                                              .Distinct()
                                              .Except(environment.PersistentDomainObjectBaseType.GetAllInterfaces())
                                              .ToList();

        foreach (var interfaceType in allSecurityInterfaces)
        {
            foreach (var interfaceProp in interfaceType.GetProperties())
            {
                var implProp = sourceType.GetImplementedProperty(interfaceProp);

                var isExplicit = environment.PropertyPathService.TryGetExpandPath(implProp).Maybe(path => path.IsEmpty);

                var name = $"{interfaceProp.Name}_Security";

                projection.Properties.Add(new ProjectionPropertyBuilder(environment, implProp.ToGetLambdaExpression(sourceType))
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
                this.FillLinkedSecurityTypes([securityType], securityTypes);
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


            return request.Concat([type]);
        }
        else
        {
            return Type.EmptyTypes;
        }
    }
}
