using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using Framework.Core;
using Framework.Parsing;
using Framework.QueryLanguage;

using ExpressionParser = Framework.Parsing.Parser<string, Framework.QueryLanguage.Expression>;
using MapFunc = System.Func<Framework.QueryLanguage.Expression, Framework.QueryLanguage.Expression>;
using ExpressionMapParser = Framework.Parsing.Parser<string, System.Func<Framework.QueryLanguage.Expression, Framework.QueryLanguage.Expression>>;

namespace Framework.OData
{
    internal class LambdaExpressionInternalParser : CharParsers
    {
        private const char EscapeChar = (char)27;
        private const string PostEscapeChars = "'";

        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly ParameterExpression _rootParameter;
        private readonly ParameterExpression _currentParameter;
        private readonly ReadOnlyCollection<ParameterExpression> _usedParameters;


        public LambdaExpressionInternalParser(NumberFormatInfo numberFormatInfo, ParameterExpression rootParameter, ParameterExpression currentParameter, ReadOnlyCollection<ParameterExpression> usedParameters)
        {
            if (numberFormatInfo == null) throw new ArgumentNullException(nameof(numberFormatInfo));
            if (rootParameter == null) throw new ArgumentNullException(nameof(rootParameter));
            if (currentParameter == null) throw new ArgumentNullException(nameof(currentParameter));
            if (usedParameters == null) throw new ArgumentNullException(nameof(usedParameters));

            this._numberFormatInfo = numberFormatInfo;
            this._rootParameter = rootParameter;
            this._currentParameter = currentParameter;
            this._usedParameters = usedParameters;
        }




        private ExpressionParser BracketsRootParser
        {
            get { return this.BetweenBrackets(this.RootBodyParser); }
        }

        public ExpressionParser RootBodyParser
        {
            get { return this.MainParser.Pipe(this.InnerRootBodyParser); }
        }

        private ExpressionParser MainParser
        {
            get { return this.GetMainParser((expression, _) => expression); }
        }


        private Parser<string, TResult> GetMainParser<TResult>(Func<Expression, bool, TResult> resultSelector)
        {
            return this.OneOfMany(this.GetLazy(() => this.BracketsRootParser.Select(res => resultSelector(res, true))),
                                  this.GetLazy(() => this.UnaryExpressionParser.Select(res => resultSelector(res, false))),
                                  this.GetLazy(() => this.OtherExpressionParser.Select(res => resultSelector(res, false))));
        }

        private LambdaExpressionInternalParser GetSubParser(ParameterExpression subParameter)
        {
            return new LambdaExpressionInternalParser(this._numberFormatInfo, this._rootParameter, subParameter, this._usedParameters.Concat(new[] { subParameter }).ToReadOnlyCollection());
        }


        private ExpressionMapParser InnerRootBodyParser
        {
            get
            {
                return this.PreSpaces(this.BinaryExpressionParser.Compose(this.GetLazy(() => this.InnerRootBodyParser)))
                           .Or(this.GetIdentity<Expression>());
            }
        }

        private ExpressionParser UnaryExpressionParser
        {
            get
            {
                Func<UnaryExpression, Expression> applyPriorityFunc = sourceExpr =>
                {
                    var req = from changeExpr in (sourceExpr.Operand as BinaryExpression).ToMaybe()

                              where changeExpr.Operation.GetPriority() < sourceExpr.Operation.GetPriority()

                              let newLeft = new UnaryExpression(sourceExpr.Operation, changeExpr.Left)

                              select new BinaryExpression(newLeft, changeExpr.Operation, changeExpr.Right);

                    return req.GetValueOrDefault((Expression)sourceExpr);
                };


                return from unaryOperation in this.PreSpaces(this.UnaryOperationParser)

                       from rootResult in this.GetMainParser((operand, isRoot) => new { Operand = operand, IsRoot = isRoot })

                       from mapOperandFunc in this.PreSpaces(this.BinaryExpressionParser).Or(this.GetIdentity<Expression>())

                       from overridePriorityFunc in rootResult.IsRoot ? this.GetIdentity<Expression>() : this.Return(applyPriorityFunc)

                       let baseExpression = new UnaryExpression(unaryOperation, mapOperandFunc(rootResult.Operand))

                       select overridePriorityFunc(baseExpression);
            }
        }



        private ExpressionMapParser BinaryExpressionParser
        {
            get
            {
                Func<BinaryExpression, Expression> applyPriorityFunc = sourceExpr =>
                {
                    var req = from changeExpr in (sourceExpr.Right as BinaryExpression).ToMaybe()

                              where changeExpr.Operation.GetPriority() < sourceExpr.Operation.GetPriority()

                              let newLeft = new BinaryExpression(sourceExpr.Left, sourceExpr.Operation, changeExpr.Left)

                              select new BinaryExpression(newLeft, changeExpr.Operation, changeExpr.Right);

                    return req.GetValueOrDefault(sourceExpr);
                };


                return from binaryOperation in this.BinaryOperationParser

                       from rootResult in this.GetMainParser((right, isRoot) => new { Right = right, IsRoot = isRoot })

                       from mapRightFunc in this.PreSpaces(this.BinaryExpressionParser).Or(this.GetIdentity<Expression>())

                       from overridePriorityFunc in rootResult.IsRoot ? this.GetIdentity<Expression>() : this.Return(applyPriorityFunc)

                       select new MapFunc(left =>
                       {
                           var baseExpression = new BinaryExpression(left, binaryOperation, mapRightFunc(rootResult.Right));

                           return overridePriorityFunc(baseExpression);
                       });
            }
        }




        private ExpressionParser OtherExpressionParser
        {
            get { return this.PreSpaces(this.InnerOtherExpressionParser); }
        }

        private ExpressionParser InnerOtherExpressionParser
        {
            get
            {
                return this.ExpandContainsExpressionParser
             .Or(() => this.PeriodMethodExpressionParser)
             .Or(() => this.StringMethodExpressionParser)
             .Or(() => this.CollectionMethodExpressionParser)
             .Or(() => this.GuidConstantExpressionParser)
             .Or(() => this.PeriodConstantExpressionParser)
             .Or(() => this.DateTimeConstantExpressionParser)
             .Or(() => this.BooleanConstantExpressionParser)
             .Or(() => this.DecimalConstantExpressionParser)
             .Or(() => this.Int32ConstantExpressionParser)
             .Or(() => this.Int64ConstantExpressionParser)
             .Or(() => this.StringConstantExpressionParser)
             .Or(() => this.NullConstantExpressionParser)
             .Or(() => this.StringConstantExpressionParser)
             .Or(() => this.DateTimePropertyParser)
             .Or(() => this.LengthPropertyParser)
             .Or(() => this.PropertyExpressionParser);
            }
        }

        private ExpressionParser ExpandContainsExpressionParser
        {
            get
            {
                return from ___ in this.BetweenSpaces(this.StringIgnoreCase("expandC"))

                       from res in this.BetweenBrackets(from expandType in this.RootBodyParser

                                                        where expandType is ConstantExpression

                                                        from _ in this.Char(',')

                                                        from filterId in this.RootBodyParser

                                                        where filterId is ConstantExpression

                                                        from __ in this.Char(',')

                                                        from source in this.RootBodyParser

                                                        select new ExpandContainsExpression(expandType as ConstantExpression, filterId as ConstantExpression, source))


                       select res;
            }
        }

        private ExpressionParser PeriodMethodExpressionParser
        {
            get
            {
                return from methodType in this.PeriodMethodExpressionTypeParser

                       from result in
                           this.BetweenBrackets(from arg1 in this.RootBodyParser

                                                from _ in this.PreSpaces(this.Char(','))

                                                from arg2 in this.RootBodyParser

                                                select new MethodExpression(arg1, methodType, new[] { arg2 }))

                       select result;
            }
        }

        private ExpressionParser StringMethodExpressionParser
        {
            get
            {
                return from methodType in this.StringMethodExpressionTypeParser

                       from result in
                           this.BetweenBrackets(from arg1 in this.RootBodyParser

                                                from _ in this.PreSpaces(this.Char(','))

                                                from arg2 in this.RootBodyParser

                                                select methodType == MethodExpressionType.StringContains

                                                     ? new MethodExpression(arg2, methodType, new[] { arg1 })

                                                     : new MethodExpression(arg1, methodType, new[] { arg2 }))

                       select result;
            }
        }

        private ExpressionParser GuidConstantExpressionParser
        {
            get
            {
                return from value in this.Between(this.GuidParser, this.StringIgnoreCase("guid'"), this.Char('\''))
                       select new GuidConstantExpression(value);
            }
        }

        private Parser<string, DateTimeConstantExpression> DateTimeConstantExpressionParser
        {
            get
            {
                return from _ in this.StringIgnoreCase("datetime'")
                       from dateStr in this.TakeTo("'")
                       from res in this.CatchParser(() => DateTime.Parse(dateStr))
                       select new DateTimeConstantExpression(res);
            }
        }

        private ExpressionParser PeriodConstantExpressionParser
        {
            get
            {
                var endDateParser = this.Pre(this.PreSpaces(this.DateTimeConstantExpressionParser), this.PreSpaces(this.Char(',')))
                                        .Select(dateTime => new DateTime?(dateTime.Value));

                return from _ in this.StringIgnoreCase("period")

                       from res in
                           this.BetweenBrackets(from startDateConst in this.DateTimeConstantExpressionParser

                                                from endDate in endDateParser.Or(this.Return(default(DateTime?)))

                                                select new Period(startDateConst.Value, endDate))

                       select new PeriodConstantExpression(res);
            }
        }


        private ExpressionParser BooleanConstantExpressionParser
        {
            get
            {
                return (this.StringIgnoreCase(bool.TrueString).Select(_ => BooleanConstantExpression.True))
              .Or(() => this.StringIgnoreCase(bool.FalseString).Select(_ => BooleanConstantExpression.False));
            }
        }

        private ExpressionParser StringConstantExpressionParser
        {
            get
            {
                return from _ in this.Char('\'')

                       from value in this.Many(this.EscapeCharParser.Or(() => this.Char(c => c != '\'')))

                       from __ in this.Char('\'')

                       select new StringConstantExpression(new string(value));
            }
        }

        private Parser<string, char> EscapeCharParser
        {
            get
            {
                return from ___ in this.Char(EscapeChar)

                       from c in this.OneOfMany(PostEscapeChars.Select(this.Char))

                       select c;

            }
        }

        public ExpressionParser PropertyPathParser
        {
            get
            {
                var propWithAlias = this.BetweenBrackets(from propertyName in this.Variable
                                                         from _ in this.Spaces1
                                                         from alias in this.Variable
                                                         select new { PropertyName = propertyName, Alias = alias }, '[', ']');

                var propWithoutAlias = this.BetweenSpaces(this.Variable)
                                           .Select(propertyName => new { PropertyName = propertyName, Alias = default(string) });



                return from properties in this.SepBy(propWithAlias.Or(propWithoutAlias), '/')

                       select properties.Aggregate((Expression)this._currentParameter, (source, propertyPair) =>

                           propertyPair.Alias == null ? new PropertyExpression(source, propertyPair.PropertyName)
                                                      : new SelectExpression(source, propertyPair.PropertyName, propertyPair.Alias));
            }
        }

        private ExpressionParser PropertyExpressionParser
        {
            get
            {
                return from startParameterInfo in this.PropertyStartParameterExpressionParser

                       from parameterNames in
                           startParameterInfo.Item2 ? this.Many(from _ in this.BetweenSpaces(this.Char('/'))
                                                                from parameterName in this.Variable
                                                                select parameterName)
                                                    : this.SepBy(this.PreSpaces(this.Variable), this.Char('/'))

                       where startParameterInfo.Item2 || parameterNames.Any()

                       select parameterNames.Aggregate((Expression)startParameterInfo.Item1, (expr, parameterName) => new PropertyExpression(expr, parameterName));
            }
        }

        private Parser<string, Tuple<ParameterExpression, bool>> PropertyStartParameterExpressionParser
        {
            get
            {
                return this.StringIgnoreCase("it").Or(() => this.StringIgnoreCase("this")).Select(_ => Tuple.Create(this._currentParameter, true))

             .Or(() => from startElementName in this.PreSpaces(this.Variable)

                       let startElementParameter = new ParameterExpression(startElementName)

                       where this._usedParameters.Contains(startElementParameter)

                       select Tuple.Create(startElementParameter, true))

             .Or(() => this.Return(Tuple.Create(this._currentParameter, false)));
            }
        }

        private ExpressionParser CollectionMethodExpressionParser
        {
            get
            {
                return from collectionMethodType in this.CollectionMethodExpressionTypeParser

                       from result in
                           this.BetweenBrackets(from source in this.RootBodyParser

                                                from subParameter in this.PreSpaces(this.Variable).Select(aliasName => new ParameterExpression(aliasName))
                                                           .Or(() => this.Return(this.GenerateAnonymousParameterExpression((source as PropertyExpression).Maybe(propExpr => propExpr.PropertyName.FromPluralize()) ?? "collection")))

                                                from _ in this.PreSpaces(this.Char(','))

                                                from bodyExpr in this.GetSubParser(subParameter).RootBodyParser

                                                let arg = new LambdaExpression(bodyExpr, new[] { subParameter })

                                                select new MethodExpression(source, collectionMethodType, new[] { arg }))

                       select result;
            }
        }

        private ExpressionParser DateTimePropertyParser
        {
            get
            {
                return this.OneOfMany(DateTimeProperties.Select(propertyName => from _ in this.StringIgnoreCase(propertyName)
                                                                                from source in this.BetweenBrackets(this.RootBodyParser)
                                                                                select new PropertyExpression(source, propertyName.ToStartUpperCase())));
            }
        }

        private ExpressionParser LengthPropertyParser
        {
            get
            {
                return from body in this.Pre(this.BetweenBrackets(this.RootBodyParser), this.StringIgnoreCase("length"))

                       select new PropertyExpression(body, "Length");
            }
        }



        private ExpressionParser DecimalConstantExpressionParser
        {
            get
            {
                return from isNegate in this.TryChar('-')
                       from numberText in this.Many1(this.Digit.Or(this.Char(CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator.Contains)))
                       from _ in this.Char('m', 'M')
                       from result in this.CatchParser(() => decimal.Parse((isNegate ? "-" : string.Empty) + new string(numberText), CultureInfo.InvariantCulture.NumberFormat))
                       select new DecimalConstantExpression(result);
            }
        }

        private ExpressionParser Int32ConstantExpressionParser
        {
            get
            {
                return from value in this.Int32Parser
                       select new Int32ConstantExpression(value);
            }
        }

        private ExpressionParser Int64ConstantExpressionParser
        {
            get
            {
                return from value in this.Int64Parser
                       select new Int64ConstantExpression(value);
            }
        }

        private ExpressionParser NullConstantExpressionParser
        {
            get
            {
                return from _ in this.StringIgnoreCase("null")
                       select NullConstantExpression.Value;
            }
        }



        private Parser<string, MethodExpressionType> StringMethodExpressionTypeParser
        {
            get { return this.FromDictionary(StringMethodExpressions, this.StringIgnoreCase); }
        }

        private Parser<string, MethodExpressionType> PeriodMethodExpressionTypeParser
        {
            get { return this.FromDictionary(PeriodMethodExpressions, this.StringIgnoreCase); }
        }


        private Parser<string, MethodExpressionType> CollectionMethodExpressionTypeParser
        {
            get { return this.FromDictionary(CollectionMethodExpressions, this.StringIgnoreCase); }
        }


        private Parser<string, UnaryOperation> UnaryOperationParser
        {
            get
            {
                return from operation in this.FromDictionary(UnaryOperations, this.StringIgnoreCase)

                       from _ in this.TestNo(this.Word1)

                       select operation;
            }
        }

        private Parser<string, BinaryOperation> BinaryOperationParser
        {
            get
            {
                return from operation in this.FromDictionary(BinaryOperations, this.StringIgnoreCase)

                       from _ in this.TestNo(this.Word1)

                       select operation;
            }
        }


        private ParameterExpression GenerateAnonymousParameterExpression(string baseName)
        {
            if (baseName == null) throw new ArgumentNullException(nameof(baseName));

            var preName = "$" + baseName.ToStartLowerCase();

            var query = from name in new[] { preName }.Concat(0.RangeInfinity().Select(index => preName + index))

                        let parameter = new ParameterExpression(name)

                        where !this._usedParameters.Contains(parameter)

                        select parameter;

            return query.First();
        }


        private static readonly Dictionary<string, MethodExpressionType> StringMethodExpressions = new Dictionary<string, MethodExpressionType>
        {
            { "startswith", MethodExpressionType.StringStartsWith },
            { "substringof", MethodExpressionType.StringContains },
            { "endswith", MethodExpressionType.StringEndsWith }
        };

        private static readonly Dictionary<string, MethodExpressionType> PeriodMethodExpressions = new Dictionary<string, MethodExpressionType>
        {
            { "containsP", MethodExpressionType.PeriodContains},
            { "isIntersectedP", MethodExpressionType.PeriodIsIntersected},
        };

        private static readonly Dictionary<string, MethodExpressionType> CollectionMethodExpressions = new Dictionary<string, MethodExpressionType>
        {
            { "any", MethodExpressionType.CollectionAny },
            { "all", MethodExpressionType.CollectionAll }
        };

        private static readonly Dictionary<string, BinaryOperation> BinaryOperations = new Dictionary<string, BinaryOperation>
        {
            { "gt"  , BinaryOperation.GreaterThan },
            { "lt"  , BinaryOperation.LessThan },

            { "le"  , BinaryOperation.LessThanOrEqual },
            { "ge"  , BinaryOperation.GreaterThanOrEqual },

            { "eq"  , BinaryOperation.Equal },
            { "ne"  , BinaryOperation.NotEqual },

            { "and" , BinaryOperation.AndAlso },
            { "or"  , BinaryOperation.OrElse },

            { "add" , BinaryOperation.Add },

            { "sub" , BinaryOperation.Subtract },
            { "mult", BinaryOperation.Mul },

            { "div" , BinaryOperation.Div },
            { "mod" , BinaryOperation.Mod }
        };

        private static readonly Dictionary<string, UnaryOperation> UnaryOperations = new Dictionary<string, UnaryOperation>
        {
            { "not"   , UnaryOperation.Not },
            { "negate", UnaryOperation.Negate },
            { "plus"  , UnaryOperation.Plus }
        };

        private static readonly ReadOnlyCollection<string> DateTimeProperties = new System.Linq.Expressions.Expression<Func<DateTime, object>>[]
        {
            d => d.Day, d => d.Hour, d => d.Minute, d => d.Month, d => d.Second, d => d.Year,
            d => d.Date // extra
        }.ToReadOnlyCollection(expr => expr.GetMemberName().ToLower());
    }
}
