using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using Anch.Core;

using Framework.CodeDom.Extend;
using Framework.Core;

namespace Framework.CodeDom.Extensions;

public static class CodeTypeReferenceExtensions
{
    public static IEnumerable<CodeTypeReference> GetReferenced(this CodeTypeReference baseTypeReference)
    {
        if (baseTypeReference is null) throw new ArgumentNullException(nameof(baseTypeReference));

        return baseTypeReference.GetAllElements(typeRef => typeRef.ArrayElementType.MaybeYield().Concat(typeRef.TypeArguments.OfType<CodeTypeReference>()));
    }

    public static CodeTypeReference ToCollectionReference(this CodeTypeReference typeArgument, Type collectionType)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(collectionType) { TypeArguments = { typeArgument } };
    }

    public static CodeTypeReference ToObservableCollectionReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return typeArgument.ToCollectionReference(typeof(ObservableCollection<>));
    }

    public static CodeTypeReference ToArrayReference(this CodeTypeReference typeArgument, int rank = 1)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeArgument, rank);
    }

    public static CodeTypeReference ToMaybeReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(Maybe<>)) { TypeArguments = { typeArgument } };
    }

    public static CodeExpression ToNothingValueExpression(this CodeTypeReference typeArgumentT)
    {
        if (typeArgumentT is null) throw new ArgumentNullException(nameof(typeArgumentT));

        return typeArgumentT.ToMaybeReference()
                            .ToTypeReferenceExpression()
                            .ToPropertyReference(nameof(Maybe<>.Nothing));
    }

    public static CodeExpression ToValueFieldReference(this CodeExpression codeExpression)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        return codeExpression.ToFieldReference("Value");
    }

    public static CodeTypeReference ToComparableReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(IComparable<>)) { TypeArguments = { typeArgument } };
    }

    public static CodeTypeReference ToEquatableReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(IEquatable<>)) { TypeArguments = { typeArgument } };
    }

    public static CodeTypeReference ToNullableReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(Nullable<>)) { TypeArguments = { typeArgument } };
    }

    public static CodeTypeReference ToListReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(List<>)) { TypeArguments = { typeArgument } };
    }

    public static CodeTypeReference ToEnumerableReference(this CodeTypeReference typeArgument)
    {
        if (typeArgument is null) throw new ArgumentNullException(nameof(typeArgument));

        return new CodeTypeReference(typeof(IEnumerable<>)) { TypeArguments = { typeArgument } };
    }



    //    public static CodeTypeReference ToArrayReference(this CodeTypeReference typeArgument)
    //    {
    //        if (typeArgument is null) throw new ArgumentNullException("typeArgument");

    //        return new CodeTypeReference(typeof(Array)) { TypeArguments = { typeArgument } };
    //    }


    public static CodeAttributeDeclaration ToAttributeDeclaration(this CodeTypeReference codeTypeReference, params CodeAttributeArgument[] arguments)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));
        if (arguments is null) throw new ArgumentNullException(nameof(arguments));


        return new CodeAttributeDeclaration(codeTypeReference, arguments);
    }

    public static CodeAttributeArgument ToAttributeArgument(this CodeExpression codeExpression)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        return new CodeAttributeArgument(codeExpression);
    }




    public static CodeTypeReference ToTypeReference(this Type type, params CodeTypeReference[] typeArguments)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (typeArguments is null) throw new ArgumentNullException(nameof(typeArguments));

        return new CodeTypeReference(type).Self(v => v.TypeArguments.AddRange(typeArguments));
    }

    public static CodeTypeReference ToTypeReference(this Type type, Type typeArgument)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.ToTypeReference(typeArgument.ToTypeReference());
    }

    public static CodeTypeReference ToTypeReference(this Type type, Type typeArgument1, Type typeArgument2)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (typeArgument2 is null) throw new ArgumentNullException(nameof(typeArgument2));

        return type.ToTypeReference(typeArgument1.ToTypeReference(), typeArgument2.ToTypeReference());
    }


    public static CodeTypeReference ToTypeReference(this Type type, Type[] typeArguments)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var newArgs = typeArguments.ToArray(t => t.ToTypeReference());

        return type.ToTypeReference(newArgs);
    }

    public static CodeTypeReference ToTypeReference(this CodeTypeReference type, params CodeTypeReference[] typeArguments)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return new CodeTypeReference(type.BaseType).Self(v => v.TypeArguments.AddRange(typeArguments));
    }

    public static CodeTypeReference ToTypeReference(this CodeTypeParameter parameter)
    {
        if (parameter is null) throw new ArgumentNullException(nameof(parameter));

        return new CodeTypeReference(parameter);
    }

    public static CodeTypeReferenceExpression ToTypeReferenceExpression(this CodeTypeParameter parameter)
    {
        if (parameter is null) throw new ArgumentNullException(nameof(parameter));

        return parameter.ToTypeReference().ToTypeReferenceExpression();
    }

    public static CodeTypeOfExpression ToTypeOfExpression(this CodeTypeParameter parameter)
    {
        if (parameter is null) throw new ArgumentNullException(nameof(parameter));

        return parameter.ToTypeReference().ToTypeOfExpression();
    }

    public static CodeTypeOfExpression ToTypeOfExpression(this CodeTypeReference codeTypeReference)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));

        return new CodeTypeOfExpression(codeTypeReference);
    }

    public static CodeTypeOfExpression ToTypeOfExpression(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.ToTypeReference().ToTypeOfExpression();
    }

    public static CodeParameterDeclarationExpression ToParameterDeclarationExpression(this CodeTypeReference codeTypeReference, string name)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));
        if (name is null) throw new ArgumentNullException(nameof(name));

        return new CodeParameterDeclarationExpression(codeTypeReference, name);
    }

    public static CodeParameterDeclarationExpression ToParameterDeclarationExpression(this CodeVariableDeclarationStatement codeVariableDeclarationStatement)
    {
        if (codeVariableDeclarationStatement is null) throw new ArgumentNullException(nameof(codeVariableDeclarationStatement));

        return codeVariableDeclarationStatement.Type.ToParameterDeclarationExpression(codeVariableDeclarationStatement.Name);
    }



    public static CodeTypeReferenceExpression ToTypeReferenceExpression(this Type type, params CodeTypeReference[] typeArguments)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.ToTypeReference(typeArguments).ToTypeReferenceExpression();
    }

    public static CodeMethodReferenceExpression ToMethodReferenceExpression(this CodeExpression codeExpression, string name, params CodeTypeReference[] typeArguments)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        return new CodeMethodReferenceExpression(codeExpression, name, typeArguments);
    }


    public static CodeMethodReferenceExpression ToMethodReferenceExpression(this CodeExpression codeExpression, string name, Type[] typeArguments)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        return codeExpression.ToMethodReferenceExpression(name, typeArguments.ToArray(a => a.ToTypeReference()));
    }

    public static CodeExpression ToMaybeReturnExpression(this CodeExpression codeExpression)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        var maybeReturnMethod = typeof(Maybe).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(Maybe.Return));

        return maybeReturnMethod.ToMethodInvokeExpression(codeExpression);
    }

    public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeMethodReferenceExpression methodReferenceExpression, params CodeExpression[] parameters)
    {
        if (methodReferenceExpression is null) throw new ArgumentNullException(nameof(methodReferenceExpression));

        return new CodeMethodInvokeExpression(methodReferenceExpression, parameters);
    }

    public static CodeMethodInvokeExpression WithNewLineParameters(this CodeMethodInvokeExpression codeMethodInvokeExpression)
    {
        if (codeMethodInvokeExpression is null) throw new ArgumentNullException(nameof(codeMethodInvokeExpression));

        codeMethodInvokeExpression.UserData[ExtendRenderConst.NewLineParameters] = true;

        return codeMethodInvokeExpression;
    }


    public static CodeTypeReferenceExpression ToTypeReferenceExpression(this CodeTypeReference codeTypeReference)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));

        return new CodeTypeReferenceExpression(codeTypeReference);
    }

    public static CodeFieldReferenceExpression ToFieldReference(this CodeExpression codeExpression, CodeMemberField field)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (field is null) throw new ArgumentNullException(nameof(field));

        return codeExpression.ToFieldReference(field.Name);
    }

    public static CodeMemberField ToMemberField(this CodeTypeReference codeTypeReference, string fieldName, CodeExpression? initExpression = null)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));
        if (fieldName is null) throw new ArgumentNullException(nameof(fieldName));

        return new CodeMemberField(codeTypeReference, fieldName) { InitExpression = initExpression };
    }

    public static CodeFieldReferenceExpression ToFieldReference(this CodeExpression codeExpression, string fieldName)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (fieldName is null) throw new ArgumentNullException(nameof(fieldName));

        return new CodeFieldReferenceExpression(codeExpression, fieldName);
    }

    public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, CodeMemberProperty property)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (property is null) throw new ArgumentNullException(nameof(property));

        return codeExpression.ToPropertyReference(property.Name);
    }

    public static CodePropertyReferenceExpression ToPropertyReference<TSource, TResult>(this CodeExpression codeExpression, Expression<Func<TSource, TResult>> expr)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));

        return codeExpression.ToPropertyReference(expr.GetMemberName());
    }

    public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, string propertyName)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));

        return new CodePropertyReferenceExpression(codeExpression, propertyName);
    }

    public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, PropertyInfo propertyInfo)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

        return new CodePropertyReferenceExpression(codeExpression, propertyInfo.Name);
    }

    public static CodeMaybePropertyReferenceExpression ToMaybePropertyReference(this CodeExpression codeExpression, PropertyInfo propertyInfo)
    {
        if (codeExpression is null) throw new ArgumentNullException(nameof(codeExpression));
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

        return new CodeMaybePropertyReferenceExpression(codeExpression, propertyInfo.Name);
    }

    public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, string methodName, IEnumerable<CodeExpression> parameters)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));
        if (methodName is null) throw new ArgumentNullException(nameof(methodName));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        return targetObject.ToMethodInvokeExpression(methodName, parameters.ToArray());
    }

    public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));
        if (methodName is null) throw new ArgumentNullException(nameof(methodName));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
    }


    public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, CodeMemberMethod method, params CodeExpression[] parameters)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));
        if (method is null) throw new ArgumentNullException(nameof(method));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        return new CodeMethodInvokeExpression(targetObject, method.Name, parameters);
    }

    public static CodeMethodInvokeExpression ToMethodInvokeExpression<TSource, TResult>(this CodeExpression targetObject, Expression<Func<TSource, TResult>> expr, params CodeExpression[] parameters)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));
        if (expr is null) throw new ArgumentNullException(nameof(expr));

        return new CodeMethodInvokeExpression(targetObject, expr.GetMemberName(), parameters);
    }

    public static CodeMethodInvokeExpression ToStaticMethodInvokeExpression(this CodeExpression targetObject, CodeMethodReferenceExpression methodReference, params CodeExpression[] parameters)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));

        return new CodeMethodInvokeExpression(methodReference, new[] { targetObject }.Concat(parameters).ToArray());
    }

    public static CodeVariableDeclarationStatement ToVariableDeclarationStatement(this CodeTypeReference typeReference, string name, CodeExpression? initialize = null)
    {
        if (typeReference is null) throw new ArgumentNullException(nameof(typeReference));
        if (name is null) throw new ArgumentNullException(nameof(name));

        return new CodeVariableDeclarationStatement(typeReference, name, initialize);
    }

    public static CodeDefaultValueExpression ToDefaultValueExpression(this CodeTypeReference typeReference)
    {
        if (typeReference is null) throw new ArgumentNullException(nameof(typeReference));

        return new CodeDefaultValueExpression(typeReference);
    }



    public static CodeStatement ToResultStatement(this CodeExpression expression, CodeTypeReference returnType)
    {
        if (expression is null) throw new ArgumentNullException(nameof(expression));
        if (returnType is null) throw new ArgumentNullException(nameof(returnType));

        return returnType.BaseType == typeof(void).ToTypeReference().BaseType ? (CodeStatement)expression.ToExpressionStatement() : expression.ToMethodReturnStatement();
    }

    public static CodeExpressionStatement ToExpressionStatement(this CodeExpression expression)
    {
        if (expression is null) throw new ArgumentNullException(nameof(expression));

        return new CodeExpressionStatement(expression);
    }

    public static CodeIsNullExpression ToIsNullExpression(this CodeExpression expression)
    {
        if (expression is null) throw new ArgumentNullException(nameof(expression));

        return new CodeIsNullExpression(expression);
    }



    public static CodeMethodReturnStatement ToMethodReturnStatement(this CodeExpression targetObject)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));

        return new CodeMethodReturnStatement(targetObject);
    }

    public static CodeMethodYieldReturnStatement ToMethodYieldReturnStatement(this CodeExpression targetObject)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));

        return new CodeMethodYieldReturnStatement { Expression = targetObject };
    }

    public static CodeStatement ToMethodReturnStatementWithLazyInitialize(this CodeExpression targetObject, CodeExpression initializeExpr)
    {
        if (targetObject is null) throw new ArgumentNullException(nameof(targetObject));

        return new CodeStatement[]
               {
                       new CodeConditionStatement
                       {
                               Condition = targetObject.ToIsNullExpression(),
                               TrueStatements =
                               {
                                       initializeExpr.ToAssignStatement(targetObject)
                               }
                       },
                       targetObject.ToMethodReturnStatement()
               }.Composite();
    }

    public static CodeObjectCreateExpression ToObjectCreateExpression(this CodeTypeReference codeTypeReference, params CodeExpression[] parameters)
    {
        if (codeTypeReference is null) throw new ArgumentNullException(nameof(codeTypeReference));

        return new CodeObjectCreateExpression(codeTypeReference, parameters);
    }

    public static CodeAssignStatement ToAssignStatement(this CodeExpression source, CodeExpression target)
    {
        if (target is null) throw new ArgumentNullException(nameof(target));
        if (source is null) throw new ArgumentNullException(nameof(source));

        return new CodeAssignStatement(target, source);
    }

    public static CodeNegateExpression ToNegateExpression(this CodeExpression source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return new CodeNegateExpression(source);
    }


    public static CodeVariableReferenceExpression ToVariableReferenceExpression(this CodeVariableDeclarationStatement source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return new CodeVariableReferenceExpression(source.Name);
    }

    public static CodeVariableReferenceExpression ToVariableReferenceExpression(this CodeParameterDeclarationExpression source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return new CodeVariableReferenceExpression(source.Name);
    }


    public static CodeTypeDeclaration MarkAsStatic(this CodeTypeDeclaration decl)
    {
        if (decl is null) throw new ArgumentNullException(nameof(decl));

        decl.UserData[UserDataMarker.IsStatic] = true;

        return decl;
    }

    public static CodeTypeDeclaration UnmarkAsStatic(this CodeTypeDeclaration decl)
    {
        if (decl is null) throw new ArgumentNullException(nameof(decl));

        decl.UserData[UserDataMarker.IsStatic] = false;

        return decl;
    }

    public static bool IsStatic(this CodeTypeDeclaration method)
    {
        var value = method.UserData[UserDataMarker.IsStatic];

        return value is bool b && b;
    }

    public static CodeMemberMethod MarkAsExtension(this CodeMemberMethod method)
    {
        if (method is null) throw new ArgumentNullException(nameof(method));

        method.UserData[UserDataMarker.IsExtension] = true;

        return method;
    }

    public static CodeMemberMethod UnmarkAsExtension(this CodeMemberMethod method)
    {
        if (method is null) throw new ArgumentNullException(nameof(method));

        method.UserData[UserDataMarker.IsExtension] = false;

        return method;
    }

    public static bool IsExtension(this CodeMemberMethod method)
    {
        var value = method.UserData[UserDataMarker.IsExtension];

        return value is bool b && b;
    }


    public static CodeStatement ToSwitchExpressionStatement(this IEnumerable<Tuple<CodeExpression, CodeStatement>> branches, CodeStatement lastElseStatement)
    {
        if (branches is null) throw new ArgumentNullException(nameof(branches));

        return branches.Reverse().Aggregate(lastElseStatement, (state, pair) => new CodeConditionStatement
        {
            Condition = pair.Item1,
            TrueStatements = { pair.Item2 },
            FalseStatements = { state }
        });
    }


    public static CodeStatement ToThrowArgumentOutOfRangeExceptionStatement(this CodeParameterDeclarationExpression parameter)
    {
        if (parameter is null) throw new ArgumentNullException(nameof(parameter));

        return new CodeThrowArgumentOutOfRangeExceptionStatement(parameter);
    }

    public static CodeStatement Composite(this IEnumerable<CodeStatement> statements)
    {
        if (statements is null) throw new ArgumentNullException(nameof(statements));

        var cachedStatements = statements.ToArray();

        if (cachedStatements.Length == 1)
        {
            return cachedStatements.Single();
        }
        else
        {
            return new CodeConditionStatement(true.ToPrimitiveExpression(), cachedStatements);
        }
    }
}

