using System;
using System.Collections.Generic;

namespace Framework.ReferencesManager;

public class ReferenceManager
{
    private static object _modifyDictionaryLock = new object();
    private static ReferenceManager _instance;
    public static ReferenceManager Instance
    {
        get { return _instance ?? (_instance = new ReferenceManager()); }
    }


    private readonly Dictionary<Tuple<Type, string>, ReferenceStorage> _type2ReferenceStorage;
    private readonly IReferenceDetector _referenceDetector;

    private ReferenceManager()
    {
        this._type2ReferenceStorage = new Dictionary<Tuple<Type, string>, ReferenceStorage>();
        this._referenceDetector = new ReferenceDetector();
    }

    /// <summary>
    /// Предоставляет перечислитель по cсылкам на требумый тип. Поиск ведется в "родной" сборке типа
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public IEnumerable<Reference> GetReferenced(Type objectType)
    {
        return this.GetReferenced(objectType, objectType.Assembly.FullName);
    }

    /// <summary>
    /// Предоставляет перечислитель по cсылкам на требумый тип в требумой сборке.
    /// "Родная" сборка игнорирутеся в данном случае
    /// </summary>
    /// <param name="objectType"></param>
    /// <param name="assemblyName">Сборка, в которой необходимо найти ссылки на тип</param>
    /// <returns></returns>
    public IEnumerable<Reference> GetReferenced(Type objectType, string assemblyName)
    {
        ReferenceStorage referenceStorage;
        var key = Tuple.Create(objectType, assemblyName);
        if (!this._type2ReferenceStorage.TryGetValue(key, out referenceStorage))
        {
            lock(_modifyDictionaryLock)
            {
                if (!this._type2ReferenceStorage.TryGetValue(key, out referenceStorage))
                {
                    referenceStorage = new ReferenceStorage(objectType, this._referenceDetector);
                    this._type2ReferenceStorage.Add(key, referenceStorage);
                }
            }
        }
        return referenceStorage[assemblyName];
    }
}
