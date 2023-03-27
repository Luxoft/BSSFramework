using System.Reflection;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL;

public partial class EntityTypeBLL
{
    public void Register([NotNull] IEnumerable<Assembly> assemblies)
    {
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        var securityContextTypes = from assembly in assemblies

                                   from type in assembly.GetTypes()

                                   where type.IsClass && !type.IsAbstract && type.IsSecurityContext()

                                   select new
                                          {
                                                  Type = type,

                                                  Id = type.GetDomainTypeId(true)
                                          };

        var entityTypes = this.GetFullList();

        var mergeResult = entityTypes.GetMergeResult(securityContextTypes, t => t.Id, pair => pair.Id);

        foreach (var pair in mergeResult.AddingItems)
        {
            this.Insert(new EntityType(true, pair.Type.IsHierarchical()) { Id = pair.Id, Name = pair.Type.Name }, pair.Id);
        }

        foreach (var pair in mergeResult.CombineItems)
        {
            var entityType = pair.Item1;

            var expectedName = pair.Item2.Type.Name;

            if (entityType.Name != expectedName)
            {
                entityType.Name = expectedName;

                this.Save(entityType);
            }
        }

        this.Remove(mergeResult.RemovingItems);
    }
}
