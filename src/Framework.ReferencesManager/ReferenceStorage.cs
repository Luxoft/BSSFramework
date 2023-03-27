namespace Framework.ReferencesManager;

class ReferenceStorage
{
    private readonly Type _objectType;
    private readonly IReferenceDetector _referenceDetector;
    private readonly Dictionary<string, IList<Reference>> _assemblyName2Refereincies;

    private readonly object _modifyDictionaryLock = new object();

    public ReferenceStorage(Type objectType, IReferenceDetector referenceDetector)
    {
        this._objectType = objectType;
        this._referenceDetector = referenceDetector;
        this._assemblyName2Refereincies = new Dictionary<string, IList<Reference>>();
    }

    internal IEnumerable<Reference> this[string assemblyName]
    {
        get
        {
            IList<Reference> result;
            if (!this._assemblyName2Refereincies.TryGetValue(assemblyName, out result))
            {
                lock (this._modifyDictionaryLock)
                {
                    if (!this._assemblyName2Refereincies.TryGetValue(assemblyName, out result))
                    {
                        result = this._referenceDetector.Find(this._objectType, assemblyName);
                        this._assemblyName2Refereincies.Add(assemblyName, result);
                    }
                }
            }
            return result;
        }
    }
}
