using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.OData;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class ODataFetchPathFactory : IFetchPathFactory<SelectOperation>
{
    private readonly Type _persistentDomainObjectBase;

    public ODataFetchPathFactory(Type persistentDomainObjectBase)
    {
        if (persistentDomainObjectBase == null) throw new ArgumentNullException(nameof(persistentDomainObjectBase));

        this._persistentDomainObjectBase = persistentDomainObjectBase;
    }


    public IEnumerable<PropertyPath> Create(Type startDomainType, SelectOperation selectOperation)
    {
        if (startDomainType == null) throw new ArgumentNullException(nameof(startDomainType));
        if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

        var allResults = selectOperation.Expands.SelectMany(
                                                            z => this.PreGetLoadPaths(startDomainType, z.GetPropertyPath().ToArray(pair => pair.Item1)))
                                        .ToList();

        var results = allResults.Distinct((arg1, arg2) => arg1.SequenceEqual(arg2)).ToList();

        return results;

        //foreach (var expand in selectOperation.Expands)
        //{
        //    foreach (var path in this.PreGetLoadPaths(this._startDomainType, expand.GetPropertyPath().ToArray(pair => pair.Item1)))
        //    {
        //         yield return path;
        //    }
        //}

        //yield break;
    }

    private IEnumerable<PropertyPath> PreGetLoadPaths(Type domainType, string[] propertyPath)
    {
        if (propertyPath.Any())
        {
            var property = domainType.GetProperty(propertyPath.First(), StringComparison.CurrentCultureIgnoreCase, true);

            if (this._persistentDomainObjectBase.IsAssignableFrom(property.PropertyType))
            {
                yield return property.GetExpandPathOrSelf();
            }
        }
    }
}
