using System;
using System.Collections.Generic;

namespace Framework.ReferencesManager;

interface IReferenceDetector
{
    IList<Reference> Find(Type type, string assemblyName);
}
