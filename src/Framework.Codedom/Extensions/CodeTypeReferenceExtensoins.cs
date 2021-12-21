using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.CodeDom
{
    public static class CodeTypeReferenceExtensions
    {
        public static IEnumerable<CodeTypeReference> GetReferenced(this CodeTypeReference baseTypeReference)
        {
            if (baseTypeReference == null) throw new ArgumentNullException(nameof(baseTypeReference));

            return baseTypeReference.GetAllElements(typeRef => typeRef.ArrayElementType.MaybeYield().Concat(typeRef.TypeArguments.OfType<CodeTypeReference>()));
        }

        public static CodeTypeReference ToCollectionReference(this CodeTypeReference typeArgument, Type collectionType)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(collectionType) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToObservableCollectionReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return typeArgument.ToCollectionReference(typeof(ObservableCollection<>));
        }

        public static CodeTypeReference ToArrayReference(this CodeTypeReference typeArgument, int rank = 1)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeArgument, rank);
        }

        public static CodeTypeReference ToMaybeReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(Maybe<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToJustReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(Just<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeExpression ToJustCodeExpression([NotNull] this CodeExpression sourceExpression, CodeTypeReference typeArgument)
        {
            if (sourceExpression == null) { throw new ArgumentNullException(nameof(sourceExpression)); }
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return typeArgument.ToJustReference().ToObjectCreateExpression(sourceExpression);
        }

        public static CodeTypeReference ToNothingReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(Nothing<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeExpression ToNothingValueExpression(this CodeTypeReference typeArgumentT)
        {
            if (typeArgumentT == null) throw new ArgumentNullException(nameof(typeArgumentT));

            return typeArgumentT.ToMaybeReference()
                                .ToTypeReferenceExpression()
                                .ToPropertyReference(nameof(Maybe<Ignore>.Nothing));
        }

        public static CodeExpression ToValueFieldReference(this CodeExpression codeExpression)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            return codeExpression.ToFieldReference("Value");
        }

        public static CodeTypeReference ToComparableReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(IComparable<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToEquatableReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(IEquatable<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToNullableReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(Nullable<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToListReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(List<>)) { TypeArguments = { typeArgument } };
        }

        public static CodeTypeReference ToEnumerableReference(this CodeTypeReference typeArgument)
        {
            if (typeArgument == null) throw new ArgumentNullException(nameof(typeArgument));

            return new CodeTypeReference(typeof(IEnumerable<>)) { TypeArguments = { typeArgument } };
        }



        //    public static CodeTypeReference ToArrayReference(this CodeTypeReference typeArgument)
        //    {
        //        if (typeArgument == null) throw new ArgumentNullException("typeArgument");

        //        return new CodeTypeReference(typeof(Array)) { TypeArguments = { typeArgument } };
        //    }


        public static CodeAttributeDeclaration ToAttributeDeclaration(this CodeTypeReference codeTypeReference, params CodeAttributeArgument[] arguments)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));


            return new CodeAttributeDeclaration(codeTypeReference, arguments);
        }

        public static CodeAttributeArgument ToAttributeArgument(this CodeExpression codeExpression)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            return new CodeAttributeArgument(codeExpression);
        }




        public static CodeTypeReference ToTypeReference(this Type type, [NotNull] params CodeTypeReference[] typeArguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

            return new CodeTypeReference(type).Self(v => v.TypeArguments.AddRange(typeArguments));
        }

        public static CodeTypeReference ToTypeReference(this Type type, [NotNull] Type typeArgument)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.ToTypeReference(typeArgument.ToTypeReference());
        }

        public static CodeTypeReference ToTypeReference(this Type type, [NotNull] Type typeArgument1, [NotNull] Type typeArgument2)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (typeArgument2 == null) throw new ArgumentNullException(nameof(typeArgument2));

            return type.ToTypeReference(typeArgument1.ToTypeReference(), typeArgument2.ToTypeReference());
        }


        public static CodeTypeReference ToTypeReference(this Type type, [NotNull] Type[] typeArguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var newArgs = typeArguments.ToArray(t => t.ToTypeReference());

            return type.ToTypeReference(newArgs);
        }

        public static CodeTypeReference ToTypeReference(this CodeTypeReference type, params CodeTypeReference[] typeArguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new CodeTypeReference(type.BaseType).Self(v => v.TypeArguments.AddRange(typeArguments));
        }

        public static CodeTypeReference ToTypeReference(this CodeTypeParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return new CodeTypeReference(parameter);
        }

        public static CodeTypeReferenceExpression ToTypeReferenceExpression(this CodeTypeParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return parameter.ToTypeReference().ToTypeReferenceExpression();
        }

        public static CodeTypeOfExpression ToTypeOfExpression(this CodeTypeParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return parameter.ToTypeReference().ToTypeOfExpression();
        }

        public static CodeTypeOfExpression ToTypeOfExpression(this CodeTypeReference codeTypeReference)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));

            return new CodeTypeOfExpression(codeTypeReference);
        }

        public static CodeTypeOfExpression ToTypeOfExpression(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.ToTypeReference().ToTypeOfExpression();
        }

        public static CodeParameterDeclarationExpression ToParameterDeclarationExpression(this CodeTypeReference codeTypeReference, string name)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new CodeParameterDeclarationExpression(codeTypeReference, name);
        }

        public static CodeParameterDeclarationExpression ToParameterDeclarationExpression(this CodeVariableDeclarationStatement codeVariableDeclarationStatement)
        {
            if (codeVariableDeclarationStatement == null) throw new ArgumentNullException(nameof(codeVariableDeclarationStatement));

            return codeVariableDeclarationStatement.Type.ToParameterDeclarationExpression(codeVariableDeclarationStatement.Name);
        }



        public static CodeTypeReferenceExpression ToTypeReferenceExpression(this Type type, params CodeTypeReference[] typeArguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.ToTypeReference(typeArguments).ToTypeReferenceExpression();
        }

        public static CodeMethodReferenceExpression ToMethodReferenceExpression(this CodeExpression codeExpression, string name, params CodeTypeReference[] typeArguments)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            return new CodeMethodReferenceExpression(codeExpression, name, typeArguments);
        }


        public static CodeMethodReferenceExpression ToMethodReferenceExpression(this CodeExpression codeExpression, string name, Type[] typeArguments)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            return codeExpression.ToMethodReferenceExpression(name, typeArguments.ToArray(a => a.ToTypeReference()));
        }

        public static CodeExpression ToMaybeReturnExpression(this CodeExpression codeExpression)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            var maybeReturnMethod = typeof(Maybe).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(Maybe.Return));

            return maybeReturnMethod.ToMethodInvokeExpression(codeExpression);
        }

        public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeMethodReferenceExpression methodReferenceExpression, params CodeExpression[] parameters)
        {
            if (methodReferenceExpression == null) throw new ArgumentNullException(nameof(methodReferenceExpression));

            return new CodeMethodInvokeExpression(methodReferenceExpression, parameters);
        }

        public static CodeMethodInvokeExpression WithNewLineParameters(this CodeMethodInvokeExpression codeMethodInvokeExpression)
        {
            if (codeMethodInvokeExpression == null) throw new ArgumentNullException(nameof(codeMethodInvokeExpression));

            codeMethodInvokeExpression.UserData[ExtendRenderConst.NewLineParameters] = true;

            return codeMethodInvokeExpression;
        }


        public static CodeTypeReferenceExpression ToTypeReferenceExpression(this CodeTypeReference codeTypeReference)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));

            return new CodeTypeReferenceExpression(codeTypeReference);
        }

        public static CodeFieldReferenceExpression ToFieldReference(this CodeExpression codeExpression, CodeMemberField field)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (field == null) throw new ArgumentNullException(nameof(field));

            return codeExpression.ToFieldReference(field.Name);
        }

        public static CodeMemberField ToMemberField(this CodeTypeReference codeTypeReference, string fieldName, CodeExpression initExpression = null)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));

            return new CodeMemberField(codeTypeReference, fieldName) { InitExpression = initExpression };
        }

        public static CodeFieldReferenceExpression ToFieldReference(this CodeExpression codeExpression, string fieldName)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));

            return new CodeFieldReferenceExpression(codeExpression, fieldName);
        }

        public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, CodeMemberProperty property)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return codeExpression.ToPropertyReference(property.Name);
        }

        public static CodePropertyReferenceExpression ToPropertyReference<TSource, TResult>(this CodeExpression codeExpression, Expression<Func<TSource, TResult>> expr)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

            return codeExpression.ToPropertyReference(expr.GetMemberName());
        }

        public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, string propertyName)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            return new CodePropertyReferenceExpression(codeExpression, propertyName);
        }

        public static CodePropertyReferenceExpression ToPropertyReference(this CodeExpression codeExpression, PropertyInfo propertyInfo)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            return new CodePropertyReferenceExpression(codeExpression, propertyInfo.Name);
        }

        public static CodeMaybePropertyReferenceExpression ToMaybePropertyReference(this CodeExpression codeExpression, PropertyInfo propertyInfo)
        {
            if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            return new CodeMaybePropertyReferenceExpression(codeExpression, propertyInfo.Name);
        }

        public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, string methodName, IEnumerable<CodeExpression> parameters)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return targetObject.ToMethodInvokeExpression(methodName, parameters.ToArray());
        }

        public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
        }


        public static CodeMethodInvokeExpression ToMethodInvokeExpression(this CodeExpression targetObject, CodeMemberMethod method, params CodeExpression[] parameters)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return new CodeMethodInvokeExpression(targetObject, method.Name, parameters);
        }

        public static CodeMethodInvokeExpression ToMethodInvokeExpression<TSource, TResult>(this CodeExpression targetObject, Expression<Func<TSource, TResult>> expr, params CodeExpression[] parameters)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return new CodeMethodInvokeExpression(targetObject, expr.GetMemberName(), parameters);
        }

        public static CodeMethodInvokeExpression ToStaticMethodInvokeExpression(this CodeExpression targetObject, CodeMethodReferenceExpression methodReference, params CodeExpression[] parameters)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));

            return new CodeMethodInvokeExpression(methodReference, new[] { targetObject }.Concat(parameters).ToArray());
        }

        public static CodeVariableDeclarationStatement ToVariableDeclarationStatement(this CodeTypeReference typeReference, string name, CodeExpression initialize = null)
        {
            if (typeReference == null) throw new ArgumentNullException(nameof(typeReference));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new CodeVariableDeclarationStatement(typeReference, name, initialize);
        }

        public static CodeDefaultValueExpression ToDefaultValueExpression(this CodeTypeReference typeReference)
        {
            if (typeReference == null) throw new ArgumentNullException(nameof(typeReference));

            return new CodeDefaultValueExpression(typeReference);
        }



        public static CodeStatement ToResultStatement(this CodeExpression expression, [NotNull] CodeTypeReference returnType)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));

            return returnType.BaseType == typeof(void).ToTypeReference().BaseType ? (CodeStatement)expression.ToExpressionStatement() : expression.ToMethodReturnStatement();
        }

        public static CodeExpressionStatement ToExpressionStatement(this CodeExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new CodeExpressionStatement(expression);
        }

        public static CodeIsNullExpression ToIsNullExpression(this CodeExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new CodeIsNullExpression(expression);
        }



        public static CodeMethodReturnStatement ToMethodReturnStatement(this CodeExpression targetObject)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));

            return new CodeMethodReturnStatement(targetObject);
        }

        public static CodeMethodYieldReturnStatement ToMethodYieldReturnStatement(this CodeExpression targetObject)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));

            return new CodeMethodYieldReturnStatement { Expression = targetObject };
        }

        public static CodeStatement ToMethodReturnStatementWithLazyInitialize(this CodeExpression targetObject, CodeExpression initializeExpr)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));

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
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));

            return new CodeObjectCreateExpression(codeTypeReference, parameters);
        }

        public static CodeAssignStatement ToAssignStatement(this CodeExpression source, CodeExpression target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CodeAssignStatement(target, source);
        }

        public static CodeNegateExpression ToNegateExpression(this CodeExpression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CodeNegateExpression(source);
        }


        public static CodeVariableReferenceExpression ToVariableReferenceExpression(this CodeVariableDeclarationStatement source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CodeVariableReferenceExpression(source.Name);
        }

        public static CodeVariableReferenceExpression ToVariableReferenceExpression(this CodeParameterDeclarationExpression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CodeVariableReferenceExpression(source.Name);
        }


        public static CodeTypeDeclaration MarkAsStatic(this CodeTypeDeclaration decl)
        {
            if (decl == null) throw new ArgumentNullException(nameof(decl));

            decl.UserData[UserDataMarker.IsStatic] = true;

            return decl;
        }

        public static CodeTypeDeclaration UnmarkAsStatic(this CodeTypeDeclaration decl)
        {
            if (decl == null) throw new ArgumentNullException(nameof(decl));

            decl.UserData[UserDataMarker.IsStatic] = false;

            return decl;
        }

        public static bool IsStatic(this CodeTypeDeclaration method)
        {
            var value = method.UserData[UserDataMarker.IsStatic];

            return value is bool b && b;
        }

        public static CodeMemberMethod MarkAsExtension([NotNull] this CodeMemberMethod method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            method.UserData[UserDataMarker.IsExtension] = true;

            return method;
        }

        public static CodeMemberMethod UnmarkAsExtension([NotNull] this CodeMemberMethod method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

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
            if (branches == null) throw new ArgumentNullException(nameof(branches));

            return branches.Reverse().Aggregate(lastElseStatement, (state, pair) => new CodeConditionStatement
            {
                Condition = pair.Item1,
                TrueStatements = { pair.Item2 },
                FalseStatements = { state }
            });
        }


        public static CodeStatement ToThrowArgumentOutOfRangeExceptionStatement(this CodeParameterDeclarationExpression parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return new CodeThrowArgumentOutOfRangeExceptionStatement(parameter);
        }

        public static CodeStatement Composite(this IEnumerable<CodeStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException(nameof(statements));

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
}
