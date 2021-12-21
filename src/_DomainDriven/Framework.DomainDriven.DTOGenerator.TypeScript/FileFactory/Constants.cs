using System;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory
{
    /// <summary>
    /// Constant options for typescript generation
    /// </summary>
    public static class Constants
    {
        public const string FromJsMethodName = "fromJs";
        public const string FromRichMethodName = "fromRich";
        public const string ToJsMethodName = "toJs";
        public const string FromObservableMethodName = "fromObservable";
        public const string ToObservableMethodName = "toObservable";
        public const string ToStrictMethodName = "toStrict";
        public const string ToUpdateMethodName = "toUpdate";
        public const string ToNativeJsonMethodName = "toNativeJson";
        public const bool InitializeByUndefined = false;
        public const string SourceVariableName = "source";
        public const string UnknownTypeName = "any";
        public const string DefaultVariableName = "e";
        public const string DTOName = "DTO";
        public const string IdentityTypeName = "Guid";
        public const string VarailableName = "x";
        public const string MapMethodName = "map";

        // TODO: drop this constants in future
        public const bool UseSecurity = false;
        public const bool GenerateOnlyReferencedEnums = false;

        public static readonly Type VoidType = typeof(void);

        public static string GenerateTypeIdenity(string typeName)
        {
            return "_" + typeName.ToLowerInvariant();
        }

        public static string PeriodToOdata(string propertyName)
        {
            return $"Core.Period.toOData({propertyName})";
        }

        public static string DateToOdata(string propertyName)
        {
            return $"Convert.toOData({propertyName})";
        }
    }
}
