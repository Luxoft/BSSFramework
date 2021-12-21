using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.ServiceModel.Async;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public partial class ServiceProxyFileFactory<TConfiguration>
    {
        private static readonly Func<string, Maybe<DTOType>> DTOParseMethod = Maybe.OfTryMethod(new TryMethod<string, DTOType>(Enum.TryParse));

        private IEnumerable<Maybe<Type>> GetRoles([NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null) { throw new ArgumentNullException(nameof(methodInfo)); }

            var methodName = methodInfo.Name;
            var arguments = methodInfo.GetParameters().ToArray(p => p.ParameterType);
            var returnType = methodInfo.ReturnType;

            yield return this.GetSaveRole(methodName, arguments, returnType);
            yield return this.GetUpdateRole(methodName, arguments, returnType);
            yield return this.GetRemoveRole(methodName, arguments, returnType);
            yield return this.GetRemoveCollectionRole(methodName, arguments, returnType);
            yield return this.GetCreateRole(methodName, arguments, returnType);
            yield return this.GetViewRole(methodName, arguments, returnType);
            yield return this.GetSourceRole(methodName, arguments, returnType);
            yield return this.GetSourceByIdentsRole(methodName, arguments, returnType);
            yield return this.GetSourceByFilterRole(methodName, arguments, returnType);

            yield return this.GetRevisionViewRole(methodName, arguments, returnType);

            yield return this.GetSecurityRole(methodName, arguments, returnType);
        }


        private Maybe<Type> GetSaveRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Save")

                   from argType in arguments.SingleMaybe()

                   let argTypeDomainName = argType.GetDomainName()

                   where argTypeDomainName == returnType.GetDomainName()

                         && argTypeDomainName == methodTail

                         && argType.IsDTOType(DTOType.StrictDTO)

                         && returnType.IsDTOType(DTOType.IdentityDTO)

                   select typeof(IAsyncSaveService<,>).MakeGenericType(argType, returnType);
        }


        private Maybe<Type> GetUpdateRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Update")

                   from argType in arguments.SingleMaybe()

                   let argTypeDomainName = argType.GetDomainName()

                   where argTypeDomainName == returnType.GetDomainName()

                         && argTypeDomainName == methodTail

                         && argType.IsDTOType(DTOType.UpdateDTO)

                         && returnType.IsDTOType(DTOType.IdentityDTO)

                   select typeof(IAsyncUpdateService<,>).MakeGenericType(argType, returnType);
        }

        private Maybe<Type> GetRemoveRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Remove")

                   from argType in arguments.SingleMaybe()

                   let argTypeDomainName = argType.GetDomainName()

                   where argTypeDomainName == methodTail

                      && returnType == typeof(void)

                      && argType.IsDTOType(DTOType.IdentityDTO)

                   select typeof(IAsyncRemoveService<>).MakeGenericType(argType);
        }

        private Maybe<Type> GetRemoveCollectionRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Remove")

                 from argType in arguments.SingleMaybe()

                 from argTypeGeneric in argType.GetCollectionOrArrayElementType().ToMaybe()

                 let argTypeGenericDomainName = argTypeGeneric.GetDomainName()

                 from methodBody in methodTail.SkipLastMaybe(argTypeGeneric.GetPluralizedDomainName())

                 where methodBody == ""

                       && returnType == typeof(void)

                       && argTypeGeneric.IsDTOType(DTOType.IdentityDTO)

                 select typeof(IAsyncRemoveCollectionService<>).MakeGenericType(argTypeGeneric);
        }

        private Maybe<Type> GetCreateRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Create")

                   from argType in arguments.SingleMaybe()

                   where returnType.GetDomainName() == methodTail

                      && argType.IsDTOType(DTOType.StrictDTO)

                      && returnType.IsDTOType(DTOType.RichDTO)

                   select typeof(IAsyncCreateService<,>).MakeGenericType(argType, returnType);
        }

        private Maybe<Type> GetViewRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Get")

                   let returnTypeDomainName = returnType.GetDomainName()

                   from methodBody in methodTail.SkipLastMaybe(returnTypeDomainName)

                   from argType in arguments.SingleMaybe()

                   let argTypeDomainName = argType.GetDomainName()

                   from dtoRole in DTOParseMethod(methodBody + "DTO")

                   where argTypeDomainName == returnTypeDomainName

                      && argType.IsDTOType(DTOType.IdentityDTO)

                      && returnType.IsDTOType(dtoRole)

                   select typeof(IAsyncViewService<,>).MakeGenericType(argType, returnType);
        }

        private Maybe<Type> GetSourceRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Get")

                   where !arguments.Any()

                   from returnTypeGeneric in returnType.GetCollectionOrArrayElementType().ToMaybe()

                   from methodBody in methodTail.SkipLastMaybe(returnTypeGeneric.GetPluralizedDomainName())

                   from dtoRole in DTOParseMethod(methodBody + "DTO")

                   where returnTypeGeneric.IsDTOType(dtoRole)

                   select typeof(IAsyncSourceService<>).MakeGenericType(returnTypeGeneric);
        }

        private Maybe<Type> GetSourceByIdentsRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodPreTail in methodName.SkipMaybe("Get")

                   from methodTail in methodPreTail.SkipLastMaybe("ByIdents")

                   from argType in arguments.SingleMaybe()

                   from argTypeGeneric in argType.GetCollectionOrArrayElementType().ToMaybe()

                   from returnTypeGeneric in returnType.GetCollectionOrArrayElementType().ToMaybe()

                   from methodBody in methodTail.SkipLastMaybe(returnTypeGeneric.GetPluralizedDomainName())

                   from dtoRole in DTOParseMethod(methodBody + "DTO")

                   where argTypeGeneric.IsDTOType(DTOType.IdentityDTO)

                      && returnTypeGeneric.IsDTOType(dtoRole)

                      && argTypeGeneric.GetDomainName() == returnTypeGeneric.GetDomainName()

                   select typeof(IAsyncSourceByIdentsService<,>).MakeGenericType(argTypeGeneric, returnTypeGeneric);
        }

        private Maybe<Type> GetSourceByFilterRole(string methodName, IEnumerable<Type> arguments, Type returnType)
        {
            return from methodPreTail in methodName.SkipMaybe("Get")

                   from argType in arguments.SingleMaybe()

                   from returnTypeGeneric in returnType.GetCollectionOrArrayElementType().ToMaybe()

                   let returnTypeGenericDomainName = returnTypeGeneric.GetDomainName()

                   from preArgDomainTypeBody in argType.GetDomainName().SkipLastMaybe("Model")

                   from argDomainTypeBody in preArgDomainTypeBody.SkipMaybe(returnTypeGenericDomainName)

                   from dtoWithMethodBody in methodPreTail.SkipLastMaybe("By" + argDomainTypeBody)

                   from methodBody in dtoWithMethodBody.SkipLastMaybe(returnTypeGeneric.GetPluralizedDomainName())

                   from dtoRole in DTOParseMethod(methodBody + "DTO")

                   where argType.IsDTOType(DTOType.StrictDTO)
                      && returnTypeGeneric.IsDTOType(dtoRole)

                   select typeof(IAsyncSourceByFilterService<,>).MakeGenericType(argType, returnTypeGeneric);
        }


        private Maybe<Type> GetRevisionViewRole(string methodName, Type[] arguments, Type returnType)
        {
            return from preMethodTail in methodName.SkipMaybe("Get")

                   from methodTail in preMethodTail.SkipLastMaybe("WithRevision")

                   let returnTypeDomainName = returnType.GetDomainName()

                   from methodBody in methodTail.SkipLastMaybe(returnTypeDomainName)

                   where arguments.Length == 2

                   let argType = arguments[0]

                   let revisionType = arguments[1]

                   let argTypeDomainName = argType.GetDomainName()

                   from dtoRole in DTOParseMethod(methodBody + "DTO")

                   where argTypeDomainName == returnTypeDomainName

                      && argType.IsDTOType(DTOType.IdentityDTO)

                      && returnType.IsDTOType(dtoRole)

                   select typeof(IAsyncRevisionViewService<,,>).MakeGenericType(argType, revisionType, returnType);
        }

        private Maybe<Type> GetSecurityRole(string methodName, Type[] arguments, Type returnType)
        {
            return from methodTail in methodName.SkipMaybe("Has")

                   from methodBody in methodTail.SkipLastMaybe("Access")

                   where arguments.Length == 2

                   let argType = arguments.First()

                   let argTypeDomainName = argType.GetDomainName()

                   where argTypeDomainName == methodBody

                      && returnType == typeof(bool)

                      && argType.IsDTOType(DTOType.IdentityDTO)

                   select typeof(IAsyncSecurityService<,>).MakeGenericType(argType, arguments[1]);
        }
    }
}
