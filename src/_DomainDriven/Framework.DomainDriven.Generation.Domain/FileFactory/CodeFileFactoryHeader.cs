using System;

namespace Framework.DomainDriven.Generation.Domain
{
    public interface ICodeFileFactoryHeader
    {
        string RelativePath { get; }

        string GetName(Type domainType);
    }

    public interface ICodeFileFactoryHeader<out TFileType> : ICodeFileFactoryHeader
    {
        TFileType Type { get; }
    }

    public class CodeFileFactoryHeader<TFileType> : ICodeFileFactoryHeader<TFileType>
    {
        private readonly Func<Type, string> _getNameFunc;


        public CodeFileFactoryHeader(TFileType type, string relativePath, Func<Type, string> getNameFunc)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (relativePath == null) throw new ArgumentNullException(nameof(relativePath));
            if (getNameFunc == null) throw new ArgumentNullException(nameof(getNameFunc));

            this._getNameFunc = getNameFunc;
            this.Type = type;
            this.RelativePath = relativePath;
        }


        public TFileType Type { get; }

        public string RelativePath { get; }


        public string GetName(Type domainType)
        {
            return this._getNameFunc(domainType);
        }

        public override string ToString()
        {
            return $"Type:{this.Type}";
        }
    }
}