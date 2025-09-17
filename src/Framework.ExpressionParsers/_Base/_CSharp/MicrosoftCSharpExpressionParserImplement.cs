using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using CommonFramework;

using Framework.Core;

namespace Framework.ExpressionParsers;

/// <summary>
/// Base on source of MS Dynamic Query Library from 2008 sdk
/// </summary>
internal static class MicrosoftCSharpExpressionParserImplement
{
    #region source not used
    internal static Type CreateClass(params DynamicProperty[] properties)
    {
        return ExpressionParserService.ClassFactory.Instance.GetDynamicClass(properties);
    }

    internal static Type CreateClass(IEnumerable<DynamicProperty> properties)
    {
        return ExpressionParserService.ClassFactory.Instance.GetDynamicClass(properties);
    }
    internal abstract class DynamicClass
    {
        public override string ToString()
        {
            var props = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < props.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(props[i].Name);
                sb.Append("=");
                sb.Append(props[i].GetValue(this, null));
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    internal class DynamicProperty
    {
        string name;
        Type type;

        public DynamicProperty(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get { return this.name; }
        }

        public Type Type
        {
            get { return this.type; }
        }
    }
    #endregion

    internal class ExpressionParserService
    {
        #region infrastructure
        internal class DynamicOrdering
        {
            public Expression Selector;
            public bool Ascending;
        }

        internal class Signature : IEquatable<Signature>
        {
            public DynamicProperty[] properties;
            public int hashCode;

            public Signature(IEnumerable<DynamicProperty> properties)
            {
                this.properties = properties.ToArray();
                this.hashCode = 0;
                foreach (DynamicProperty p in properties)
                {
                    this.hashCode ^= p.Name.GetHashCode() ^ p.Type.GetHashCode();
                }
            }

            public override int GetHashCode()
            {
                return this.hashCode;
            }

            public override bool Equals(object obj)
            {
                return obj is Signature ? this.Equals((Signature)obj) : false;
            }

            public bool Equals(Signature other)
            {
                if (this.properties.Length != other.properties.Length) return false;
                for (int i = 0; i < this.properties.Length; i++)
                {
                    if (this.properties[i].Name != other.properties[i].Name || this.properties[i].Type != other.properties[i].Type) return false;
                }
                return true;
            }
        }

        internal class ClassFactory
        {
            public static readonly ClassFactory Instance = new ClassFactory();

            static ClassFactory() { }  // Trigger lazy initialization of static fields

            readonly ModuleBuilder _module;
            readonly Dictionary<Signature, Type> _classes;
            int _classCount;
            readonly ReaderWriterLock _rwLock;

            private ClassFactory()
            {
                AssemblyName name = new AssemblyName("DynamicClasses");
                AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
#if ENABLE_LINQ_PARTIAL_TRUST
            new ReflectionPermission(PermissionState.Unrestricted).Assert();
#endif
                try
                {
                    this._module = assembly.DefineDynamicModule("Module");
                }
                finally
                {
#if ENABLE_LINQ_PARTIAL_TRUST
                PermissionSet.RevertAssert();
#endif
                }
                this._classes = new Dictionary<Signature, Type>();
                this._rwLock = new ReaderWriterLock();
            }

            public Type GetDynamicClass(IEnumerable<DynamicProperty> properties)
            {
                this._rwLock.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    Signature signature = new Signature(properties);
                    Type type;
                    if (!this._classes.TryGetValue(signature, out type))
                    {
                        type = this.CreateDynamicClass(signature.properties);
                        this._classes.Add(signature, type);
                    }
                    return type;
                }
                finally
                {
                    this._rwLock.ReleaseReaderLock();
                }
            }

            Type CreateDynamicClass(DynamicProperty[] properties)
            {
                LockCookie cookie = this._rwLock.UpgradeToWriterLock(Timeout.Infinite);
                try
                {
                    string typeName = "DynamicClass" + (this._classCount + 1);
#if ENABLE_LINQ_PARTIAL_TRUST
                new ReflectionPermission(PermissionState.Unrestricted).Assert();
#endif
                    try
                    {
                        TypeBuilder tb = this._module.DefineType(typeName, TypeAttributes.Class |
                                                                           TypeAttributes.Public, typeof(DynamicClass));
                        FieldInfo[] fields = this.GenerateProperties(tb, properties);
                        this.GenerateEquals(tb, fields);
                        this.GenerateGetHashCode(tb, fields);
                        Type result = tb.CreateTypeInfo();
                        this._classCount++;
                        return result;
                    }
                    finally
                    {
#if ENABLE_LINQ_PARTIAL_TRUST
                    PermissionSet.RevertAssert();
#endif
                    }
                }
                finally
                {
                    this._rwLock.DowngradeFromWriterLock(ref cookie);
                }
            }

            FieldInfo[] GenerateProperties(TypeBuilder tb, DynamicProperty[] properties)
            {
                FieldInfo[] fields = new FieldBuilder[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    var dp = properties[i];
                    var fb = tb.DefineField("_" + dp.Name, dp.Type, FieldAttributes.Private);
                    var pb = tb.DefineProperty(dp.Name, PropertyAttributes.HasDefault, dp.Type, null);
                    var mbGet = tb.DefineMethod("get_" + dp.Name,
                                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                dp.Type, Type.EmptyTypes);
                    var genGet = mbGet.GetILGenerator();
                    genGet.Emit(OpCodes.Ldarg_0);
                    genGet.Emit(OpCodes.Ldfld, fb);
                    genGet.Emit(OpCodes.Ret);
                    var mbSet = tb.DefineMethod("set_" + dp.Name,
                                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                null, new Type[] { dp.Type });
                    var genSet = mbSet.GetILGenerator();
                    genSet.Emit(OpCodes.Ldarg_0);
                    genSet.Emit(OpCodes.Ldarg_1);
                    genSet.Emit(OpCodes.Stfld, fb);
                    genSet.Emit(OpCodes.Ret);
                    pb.SetGetMethod(mbGet);
                    pb.SetSetMethod(mbSet);
                    fields[i] = fb;
                }
                return fields;
            }

            void GenerateEquals(TypeBuilder tb, FieldInfo[] fields)
            {
                var mb = tb.DefineMethod("Equals",
                                         MethodAttributes.Public | MethodAttributes.ReuseSlot |
                                         MethodAttributes.Virtual | MethodAttributes.HideBySig,
                                         typeof(bool), new Type[] { typeof(object) });
                var gen = mb.GetILGenerator();
                var other = gen.DeclareLocal(tb);
                var next = gen.DefineLabel();
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Isinst, tb);
                gen.Emit(OpCodes.Stloc, other);
                gen.Emit(OpCodes.Ldloc, other);
                gen.Emit(OpCodes.Brtrue_S, next);
                gen.Emit(OpCodes.Ldc_I4_0);
                gen.Emit(OpCodes.Ret);
                gen.MarkLabel(next);
                foreach (FieldInfo field in fields)
                {
                    Type ft = field.FieldType;
                    Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                    next = gen.DefineLabel();
                    gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.Emit(OpCodes.Ldloc, other);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("Equals", new Type[] { ft, ft }), null);
                    gen.Emit(OpCodes.Brtrue_S, next);
                    gen.Emit(OpCodes.Ldc_I4_0);
                    gen.Emit(OpCodes.Ret);
                    gen.MarkLabel(next);
                }
                gen.Emit(OpCodes.Ldc_I4_1);
                gen.Emit(OpCodes.Ret);
            }

            void GenerateGetHashCode(TypeBuilder tb, FieldInfo[] fields)
            {
                var mb = tb.DefineMethod("GetHashCode",
                                         MethodAttributes.Public | MethodAttributes.ReuseSlot |
                                         MethodAttributes.Virtual | MethodAttributes.HideBySig,
                                         typeof(int), Type.EmptyTypes);
                var gen = mb.GetILGenerator();
                gen.Emit(OpCodes.Ldc_I4_0);
                foreach (FieldInfo field in fields)
                {
                    Type ft = field.FieldType;
                    Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                    gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("GetHashCode", new Type[] { ft }), null);
                    gen.Emit(OpCodes.Xor);
                }
                gen.Emit(OpCodes.Ret);
            }
        }



        abstract class FindMethodsResult
        {
            public static FindMethodsResult CreateSimple(MethodInfo methodInfo)
            {
                if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

                return new SimpleResult(methodInfo);
            }

            public static FindMethodsResult CreateMulty(int count)
            {
                return new MultyResult(count);
            }
            public static FindMethodsResult CreateNone()
            {
                return NoneResult.Instance;
            }

            public abstract int Count { get; }

            public class NoneResult : FindMethodsResult
            {
                internal static NoneResult Instance = new NoneResult();
                private NoneResult()
                {
                }

                public override int Count
                {
                    get { return 0; }
                }
            }

            public abstract class SuccessedResult : FindMethodsResult
            {
                private readonly MethodInfo _methodInfo;

                protected SuccessedResult(MethodInfo methodInfo)
                {
                    this._methodInfo = methodInfo;
                }

                public MethodInfo MethodInfo
                {
                    get { return this._methodInfo; }
                }
            }
            public class SimpleResult : SuccessedResult
            {

                internal SimpleResult(MethodInfo methodInfo)
                        : base(methodInfo)
                {
                }

                public override int Count
                {
                    get { return 1; }
                }

            }
            public class MultyResult : FindMethodsResult
            {
                private readonly int _count;

                internal MultyResult(int count)
                {
                    this._count = count;
                }

                public override int Count
                {
                    get { return this._count; }
                }
            }
            public class ExtensionResult : SuccessedResult
            {
                private readonly Type _declareType;
                private readonly Type _genericType;


                internal ExtensionResult(MethodInfo methodInfo, Type declareType, Type genericType)
                        : base(methodInfo)
                {
                    this._declareType = declareType;
                    this._genericType = genericType;
                }

                public override int Count
                {
                    get { return 1; }
                }

                public Type DeclareType
                {
                    get { return this._declareType; }
                }

                public Type GenericType
                {
                    get { return this._genericType; }
                }
            }

            public static FindMethodsResult CreateExtension(Type declareType, MethodInfo methodInfo, Type genericType)
            {
                return new ExtensionResult(methodInfo, declareType, genericType);
            }
        }

        struct Token
        {
            public TokenId id;
            public string text;
            public int pos;
        }

        enum TokenId
        {
            Unknown,
            End,
            Identifier,
            StringLiteral,
            IntegerLiteral,
            RealLiteral,
            Exclamation,
            Percent,
            Amphersand,
            OpenParen,
            CloseParen,
            Asterisk,
            Plus,
            Comma,
            Minus,
            Dot,
            Slash,
            Colon,
            LessThan,
            Equal,
            GreaterThan,
            Question,
            OpenBracket,
            CloseBracket,
            Bar,
            ExclamationEqual,
            DoubleAmphersand,
            LessThanEqual,
            LessGreater,
            DoubleEqual,
            GreaterThanEqual,
            DoubleBar
        }

        interface ILogicalSignatures
        {
            void F(bool x, bool y);
            void F(bool? x, bool? y);
        }

        interface IArithmeticSignatures
        {
            void F(int x, int y);
            void F(uint x, uint y);
            void F(long x, long y);
            void F(ulong x, ulong y);
            void F(float x, float y);
            void F(double x, double y);
            void F(decimal x, decimal y);
            void F(int? x, int? y);
            void F(uint? x, uint? y);
            void F(long? x, long? y);
            void F(ulong? x, ulong? y);
            void F(float? x, float? y);
            void F(double? x, double? y);
            void F(decimal? x, decimal? y);
        }

        interface IRelationalSignatures : IArithmeticSignatures
        {
            void F(string x, string y);
            void F(char x, char y);
            void F(DateTime x, DateTime y);
            void F(TimeSpan x, TimeSpan y);
            void F(char? x, char? y);
            void F(DateTime? x, DateTime? y);
            void F(TimeSpan? x, TimeSpan? y);
        }

        interface IEqualitySignatures : IRelationalSignatures
        {
            void F(bool x, bool y);
            void F(bool? x, bool? y);
        }

        interface IAddSignatures : IArithmeticSignatures
        {
            void F(DateTime x, TimeSpan y);
            void F(TimeSpan x, TimeSpan y);
            void F(DateTime? x, TimeSpan? y);
            void F(TimeSpan? x, TimeSpan? y);
        }

        interface ISubtractSignatures : IAddSignatures
        {
            void F(DateTime x, DateTime y);
            void F(DateTime? x, DateTime? y);
        }

        interface INegationSignatures
        {
            void F(int x);
            void F(long x);
            void F(float x);
            void F(double x);
            void F(decimal x);
            void F(int? x);
            void F(long? x);
            void F(float? x);
            void F(double? x);
            void F(decimal? x);
        }

        interface INotSignatures
        {
            void F(bool x);
            void F(bool? x);
        }

        #endregion

        static readonly Type[] predefinedTypes = {
                                                         typeof(object),
                                                         typeof(bool),
                                                         typeof(char),
                                                         typeof(string),
                                                         typeof(sbyte),
                                                         typeof(byte),
                                                         typeof(short),
                                                         typeof(ushort),
                                                         typeof(int),
                                                         typeof(uint),
                                                         typeof(long),
                                                         typeof(ulong),
                                                         typeof(float),
                                                         typeof(double),
                                                         typeof(decimal),
                                                         typeof(DateTime),
                                                         typeof(TimeSpan),
                                                         typeof(Guid),
                                                         typeof(Math),
                                                         typeof(Convert)
                                                 };

        static readonly Expression trueLiteral = Expression.Constant(true);
        static readonly Expression falseLiteral = Expression.Constant(false);
        static readonly Expression nullLiteral = Expression.Constant(null);

        static readonly string keywordIt = "it";
        static readonly string keywordIif = "iif";
        static readonly string keywordNew = "new";

        static Dictionary<string, object> keywords;

        readonly Dictionary<string, object> symbols;
        IDictionary<string, object> _externals;
        readonly Dictionary<Expression, string> literals;

        ParameterExpression _inputParameter;

        readonly string _text;
        int _textPos;
        readonly int _textLen;
        private char _currentChar;
        Token _token;

        public ExpressionParserService(ParameterExpression[] parameters, string expression, object[] values)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (keywords == null) keywords = CreateKeywords();
            this.symbols = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.literals = new Dictionary<Expression, string>();
            if (parameters != null)
            {
                this.ProcessParameters(parameters);
            }
            if (values != null)
            {
                this.ProcessValues(values);
            }
            this._text = expression;
            this._textLen = this._text.Length;
            this.SetTextPos(0);
            this.NextToken();
        }

        void ProcessParameters(ParameterExpression[] parameters)
        {
            foreach (var pe in parameters.Where(pe => !string.IsNullOrEmpty(pe.Name)))
            {
                this.AddSymbol(pe.Name, pe);
            }
            if (parameters.Length == 1 && string.IsNullOrEmpty(parameters[0].Name))
            {
                this._inputParameter = parameters[0];
            }
        }

        void ProcessValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];
                if (i == values.Length - 1 && value is IDictionary<string, object>)
                {
                    this._externals = (IDictionary<string, object>)value;
                }
                else
                {
                    this.AddSymbol("@" + i.ToString(CultureInfo.InvariantCulture), value);
                }
            }
        }

        void AddSymbol(string name, object value)
        {
            if (this.symbols.ContainsKey(name))
            {
                throw this.ParseError(ExpressionParsersResources.DuplicateIdentifier, name);
            }
            this.symbols.Add(name, value);
        }

        public Expression Parse(Type resultType)
        {
            var exprPos = this._token.pos;
            var expr = this.ParseExpression();
            if (resultType != null && (expr = this.PromoteExpression(expr, resultType, true)) == null)
            {
                throw this.ParseError(exprPos, ExpressionParsersResources.ExpressionTypeMismatch, GetTypeName(resultType));
            }
            this.ValidateToken(TokenId.End, ExpressionParsersResources.SyntaxError);
            return expr;
        }

#pragma warning disable 0219
        public IEnumerable<DynamicOrdering> ParseOrdering()
        {
            List<DynamicOrdering> orderings = new List<DynamicOrdering>();
            while (true)
            {
                Expression expr = this.ParseExpression();
                bool ascending = true;
                if (this.TokenIdentifierIs("asc") || this.TokenIdentifierIs("ascending"))
                {
                    this.NextToken();
                }
                else if (this.TokenIdentifierIs("desc") || this.TokenIdentifierIs("descending"))
                {
                    this.NextToken();
                    ascending = false;
                }
                orderings.Add(new DynamicOrdering { Selector = expr, Ascending = ascending });
                if (this._token.id != TokenId.Comma) break;
                this.NextToken();
            }
            this.ValidateToken(TokenId.End, ExpressionParsersResources.SyntaxError);
            return orderings;
        }
#pragma warning restore 0219

        // ?: operator
        Expression ParseExpression()
        {
            int errorPos = this._token.pos;
            Expression expr = this.ParseLogicalOr();
            if (this._token.id == TokenId.Question)
            {
                this.NextToken();
                Expression expr1 = this.ParseExpression();
                this.ValidateToken(TokenId.Colon, ExpressionParsersResources.ColonExpected);
                this.NextToken();
                Expression expr2 = this.ParseExpression();
                expr = this.GenerateConditional(expr, expr1, expr2, errorPos);
            }
            return expr;
        }

        // ||, or operator
        Expression ParseLogicalOr()
        {
            Expression left = this.ParseLogicalAnd();
            while (this._token.id == TokenId.DoubleBar || this.TokenIdentifierIs("or"))
            {
                Token op = this._token;
                this.NextToken();
                Expression right = this.ParseLogicalAnd();
                this.CheckAndPromoteOperands(typeof(ILogicalSignatures), op.text, ref left, ref right, op.pos);
                left = Expression.OrElse(left, right);
            }
            return left;
        }

        // &&, and operator
        Expression ParseLogicalAnd()
        {
            Expression left = this.ParseComparison();
            while (this._token.id == TokenId.DoubleAmphersand || this.TokenIdentifierIs("and"))
            {
                Token op = this._token;
                this.NextToken();
                Expression right = this.ParseComparison();
                this.CheckAndPromoteOperands(typeof(ILogicalSignatures), op.text, ref left, ref right, op.pos);
                left = Expression.AndAlso(left, right);
            }
            return left;
        }

        // =, ==, !=, <>, >, >=, <, <= operators
        Expression ParseComparison()
        {
            Expression left = this.ParseAdditive();
            while (this._token.id == TokenId.Equal || this._token.id == TokenId.DoubleEqual || this._token.id == TokenId.ExclamationEqual || this._token.id == TokenId.LessGreater || this._token.id == TokenId.GreaterThan || this._token.id == TokenId.GreaterThanEqual || this._token.id == TokenId.LessThan || this._token.id == TokenId.LessThanEqual)
            {
                Token op = this._token;
                this.NextToken();
                Expression right = this.ParseAdditive();
                bool isEquality = op.id == TokenId.Equal || op.id == TokenId.DoubleEqual ||
                                  op.id == TokenId.ExclamationEqual || op.id == TokenId.LessGreater;
                if (isEquality && !left.Type.IsValueType && !right.Type.IsValueType)
                {
                    if (left.Type != right.Type)
                    {
                        if (left.Type.IsAssignableFrom(right.Type))
                        {
                            right = Expression.Convert(right, left.Type);
                        }
                        else if (right.Type.IsAssignableFrom(left.Type))
                        {
                            left = Expression.Convert(left, right.Type);
                        }
                        else
                        {
                            throw this.IncompatibleOperandsError(op.text, left, right, op.pos);
                        }
                    }
                }
                else if (IsEnumType(left.Type) || IsEnumType(right.Type))
                {
                    if (left.Type != right.Type)
                    {
                        Expression e;
                        if ((e = this.PromoteExpression(right, left.Type, true)) != null)
                        {
                            right = e;
                        }
                        else if ((e = this.PromoteExpression(left, right.Type, true)) != null)
                        {
                            left = e;
                        }
                        else
                        {
                            throw this.IncompatibleOperandsError(op.text, left, right, op.pos);
                        }
                    }
                }
                else
                {
                    this.CheckAndPromoteOperands(isEquality ? typeof(IEqualitySignatures) : typeof(IRelationalSignatures),
                                                 op.text, ref left, ref right, op.pos);
                }
                switch (op.id)
                {
                    case TokenId.Equal:
                    case TokenId.DoubleEqual:
                        left = this.GenerateEqual(left, right);
                        break;
                    case TokenId.ExclamationEqual:
                    case TokenId.LessGreater:
                        left = this.GenerateNotEqual(left, right);
                        break;
                    case TokenId.GreaterThan:
                        left = this.GenerateGreaterThan(left, right);
                        break;
                    case TokenId.GreaterThanEqual:
                        left = this.GenerateGreaterThanEqual(left, right);
                        break;
                    case TokenId.LessThan:
                        left = this.GenerateLessThan(left, right);
                        break;
                    case TokenId.LessThanEqual:
                        left = this.GenerateLessThanEqual(left, right);
                        break;
                }
            }
            return left;
        }

        // +, -, & operators
        Expression ParseAdditive()
        {
            Expression left = this.ParseMultiplicative();
            while (this._token.id == TokenId.Plus || this._token.id == TokenId.Minus || this._token.id == TokenId.Amphersand)
            {
                Token op = this._token;
                this.NextToken();
                Expression right = this.ParseMultiplicative();
                switch (op.id)
                {
                    case TokenId.Plus:
                        if (left.Type == typeof(string) || right.Type == typeof(string))
                            goto case TokenId.Amphersand;
                        this.CheckAndPromoteOperands(typeof(IAddSignatures), op.text, ref left, ref right, op.pos);
                        left = this.GenerateAdd(left, right);
                        break;
                    case TokenId.Minus:
                        this.CheckAndPromoteOperands(typeof(ISubtractSignatures), op.text, ref left, ref right, op.pos);
                        left = this.GenerateSubtract(left, right);
                        break;
                    case TokenId.Amphersand:
                        left = this.GenerateStringConcat(left, right);
                        break;
                }
            }
            return left;
        }

        // *, /, %, mod operators
        Expression ParseMultiplicative()
        {
            Expression left = this.ParseUnary();
            while (this._token.id == TokenId.Asterisk || this._token.id == TokenId.Slash || this._token.id == TokenId.Percent || this.TokenIdentifierIs("mod"))
            {
                Token op = this._token;
                this.NextToken();
                Expression right = this.ParseUnary();
                this.CheckAndPromoteOperands(typeof(IArithmeticSignatures), op.text, ref left, ref right, op.pos);
                switch (op.id)
                {
                    case TokenId.Asterisk:
                        left = Expression.Multiply(left, right);
                        break;
                    case TokenId.Slash:
                        left = Expression.Divide(left, right);
                        break;
                    case TokenId.Percent:
                    case TokenId.Identifier:
                        left = Expression.Modulo(left, right);
                        break;
                }
            }
            return left;
        }

        // -, !, not unary operators
        Expression ParseUnary()
        {
            if (this._token.id == TokenId.Minus || this._token.id == TokenId.Exclamation || this.TokenIdentifierIs("not"))
            {
                Token op = this._token;
                this.NextToken();
                if (op.id == TokenId.Minus && (this._token.id == TokenId.IntegerLiteral || this._token.id == TokenId.RealLiteral))
                {
                    this._token.text = "-" + this._token.text;
                    this._token.pos = op.pos;
                    return this.ParsePrimary();
                }
                Expression expr = this.ParseUnary();
                if (op.id == TokenId.Minus)
                {
                    this.CheckAndPromoteOperand(typeof(INegationSignatures), op.text, ref expr, op.pos);
                    expr = Expression.Negate(expr);
                }
                else
                {
                    this.CheckAndPromoteOperand(typeof(INotSignatures), op.text, ref expr, op.pos);
                    expr = Expression.Not(expr);
                }
                return expr;
            }
            return this.ParsePrimary();
        }

        Expression ParsePrimary()
        {
            Expression expr = this.ParsePrimaryStart();
            while (true)
            {
                if (this._token.id == TokenId.Dot)
                {
                    this.NextToken();
                    expr = this.ParseMemberAccess(null, expr);
                }
                else if (this._token.id == TokenId.OpenBracket)
                {
                    expr = this.ParseElementAccess(expr);
                }
                else
                {
                    break;
                }
            }
            return expr;
        }

        Expression ParsePrimaryStart()
        {
            switch (this._token.id)
            {
                case TokenId.Identifier:
                    return this.ParseIdentifier();
                case TokenId.StringLiteral:
                    return this.ParseStringLiteral();
                case TokenId.IntegerLiteral:
                    return this.ParseIntegerLiteral();
                case TokenId.RealLiteral:
                    return this.ParseRealLiteral();
                case TokenId.OpenParen:
                    return this.ParseParenExpression();
                default:
                    throw this.ParseError(ExpressionParsersResources.ExpressionExpected);
            }
        }

        Expression ParseStringLiteral()
        {
            this.ValidateToken(TokenId.StringLiteral);
            char quote = this._token.text[0];
            string s = this._token.text.Substring(1, this._token.text.Length - 2);
            int start = 0;
            while (true)
            {
                int i = s.IndexOf(quote, start);
                if (i < 0) break;
                s = s.Remove(i, 1);
                start = i + 1;
            }
            if (quote == '\'')
            {
                if (s.Length != 1)
                    throw this.ParseError(ExpressionParsersResources.InvalidCharacterLiteral);
                this.NextToken();
                return this.CreateLiteral(s[0], s);
            }
            this.NextToken();
            return this.CreateLiteral(s, s);
        }

        Expression ParseIntegerLiteral()
        {
            this.ValidateToken(TokenId.IntegerLiteral);
            string text = this._token.text;
            if (text[0] != '-')
            {
                ulong value;
                if (!ulong.TryParse(text, out value))
                    throw this.ParseError(ExpressionParsersResources.InvalidIntegerLiteral, text);
                this.NextToken();
                if (value <= (ulong)int.MaxValue) return this.CreateLiteral((int)value, text);
                if (value <= (ulong)uint.MaxValue) return this.CreateLiteral((uint)value, text);
                if (value <= (ulong)long.MaxValue) return this.CreateLiteral((long)value, text);
                return this.CreateLiteral(value, text);
            }
            else
            {
                long value;
                if (!long.TryParse(text, out value))
                    throw this.ParseError(ExpressionParsersResources.InvalidIntegerLiteral, text);
                this.NextToken();
                if (value >= int.MinValue && value <= int.MaxValue)
                    return this.CreateLiteral((int)value, text);
                return this.CreateLiteral(value, text);
            }
        }

        Expression ParseRealLiteral()
        {
            this.ValidateToken(TokenId.RealLiteral);
            string text = this._token.text;
            object value = null;
            char last = text[text.Length - 1];
            if (last == 'F' || last == 'f')
            {
                float f;
                if (float.TryParse(text.Substring(0, text.Length - 1), out f))
                {
                    value = f;
                }
            }
            else
            {
                double d;
                if (double.TryParse(text, out d))
                {
                    value = d;
                }
            }
            if (value == null)
            {
                throw this.ParseError(ExpressionParsersResources.InvalidRealLiteral, text);
            }
            this.NextToken();
            return this.CreateLiteral(value, text);
        }

        Expression CreateLiteral(object value, string text)
        {
            ConstantExpression expr = Expression.Constant(value);
            this.literals.Add(expr, text);
            return expr;
        }

        Expression ParseParenExpression()
        {
            this.ValidateToken(TokenId.OpenParen, ExpressionParsersResources.OpenParenExpected);
            this.NextToken();
            var e = this.ParseExpression();
            this.ValidateToken(TokenId.CloseParen, ExpressionParsersResources.CloseParenOrOperatorExpected);
            this.NextToken();
            return e;
        }

        Expression ParseIdentifier()
        {
            this.ValidateToken(TokenId.Identifier);
            object value;
            if (keywords.TryGetValue(this._token.text, out value))
            {
                if (value is Type) return this.ParseTypeAccess((Type)value);
                if (value == (object)keywordIt) return this.ParseIt();
                if (value == (object)keywordIif) return this.ParseIif();
                if (value == (object)keywordNew) return this.ParseNew();
                this.NextToken();
                return (Expression)value;
            }
            if (this.symbols.TryGetValue(this._token.text, out value) || this._externals != null && this._externals.TryGetValue(this._token.text, out value))
            {
                var expr = value as Expression;
                if (expr == null)
                {
                    expr = Expression.Constant(value);
                }
                else
                {
                    var lambda = expr as LambdaExpression;
                    if (lambda != null)
                    {
                        return this.ParseExpressionInvocation(lambda);
                    }
                }
                this.NextToken();
                return expr;
            }
            if (this._inputParameter != null)
            {
                return this.ParseMemberAccess(null, this._inputParameter);
            }

            throw this.ParseError(ExpressionParsersResources.UnknownIdentifier, this._token.text);
        }

        Expression ParseIt()
        {
            if (this._inputParameter == null)
            {
                throw this.ParseError(ExpressionParsersResources.NoItInScope);
            }
            this.NextToken();
            return this._inputParameter;
        }

        Expression ParseIif()
        {
            int errorPos = this._token.pos;
            this.NextToken();
            Expression[] args = this.ParseArgumentList();
            if (args.Length != 3)
                throw this.ParseError(errorPos, ExpressionParsersResources.IifRequiresThreeArgs);
            return this.GenerateConditional(args[0], args[1], args[2], errorPos);
        }

        Expression GenerateConditional(Expression test, Expression expr1, Expression expr2, int errorPos)
        {
            if (test.Type != typeof(bool))
                throw this.ParseError(errorPos, ExpressionParsersResources.FirstExprMustBeBool);
            if (expr1.Type != expr2.Type)
            {
                Expression expr1as2 = expr2 != nullLiteral ? this.PromoteExpression(expr1, expr2.Type, true) : null;
                Expression expr2as1 = expr1 != nullLiteral ? this.PromoteExpression(expr2, expr1.Type, true) : null;
                if (expr1as2 != null && expr2as1 == null)
                {
                    expr1 = expr1as2;
                }
                else if (expr2as1 != null && expr1as2 == null)
                {
                    expr2 = expr2as1;
                }
                else
                {
                    string type1 = expr1 != nullLiteral ? expr1.Type.Name : "null";
                    string type2 = expr2 != nullLiteral ? expr2.Type.Name : "null";
                    if (expr1as2 != null && expr2as1 != null)
                        throw this.ParseError(errorPos, ExpressionParsersResources.BothTypesConvertToOther, type1, type2);
                    throw this.ParseError(errorPos, ExpressionParsersResources.NeitherTypeConvertsToOther, type1, type2);
                }
            }
            return Expression.Condition(test, expr1, expr2);
        }

        Expression ParseNew()
        {
            this.NextToken();
            return this.ParseDynamicType();
            //if (_token.id == TokenId.OpenParen)
            //{

            //}
            //var type = Type.GetType(_token.text);
            //IEnumerable<Expression> expressions = ParseArguments();
            //Expression[] arguments = ParseArguments();
            //var result = FindBestMethod(type.GetConstructors(), arguments);
            //if (result is FindMethodsResult.SimpleResult)
            //{
            //    return Expression.New((ConstructorInfo)((MethodBase)((FindMethodsResult.SimpleResult)result).MethodInfo), arguments);
            //}
            //throw new Exception();
        }

        private Expression ParseDynamicType()
        {
            this.ValidateToken(TokenId.OpenParen, ExpressionParsersResources.OpenParenExpected);
            this.NextToken();
            var properties = new List<DynamicProperty>();
            var expressions = new List<Expression>();
            while (true)
            {
                int exprPos = this._token.pos;
                Expression expr = this.ParseExpression();
                string propName;
                if (this.TokenIdentifierIs("as"))
                {
                    this.NextToken();
                    propName = this.GetIdentifier();
                    this.NextToken();
                }
                else
                {
                    var me = expr as MemberExpression;
                    if (me == null)
                    {
                        throw this.ParseError(exprPos, ExpressionParsersResources.MissingAsClause);
                    }
                    propName = me.Member.Name;
                }
                expressions.Add(expr);
                properties.Add(new DynamicProperty(propName, expr.Type));
                if (this._token.id != TokenId.Comma) break;
                this.NextToken();
            }
            this.ValidateToken(TokenId.CloseParen, ExpressionParsersResources.CloseParenOrCommaExpected);
            this.NextToken();
            var type = CreateClass(properties);
            var bindings = new MemberBinding[properties.Count];
            for (int i = 0; i < bindings.Length; i++)
            {
                bindings[i] = Expression.Bind(type.GetProperty(properties[i].Name), expressions[i]);
            }
            return Expression.MemberInit(Expression.New(type), bindings);
        }

        Expression ParseExpressionInvocation(LambdaExpression lambda)
        {
            int errorPos = this._token.pos;
            this.NextToken();
            var args = this.ParseArgumentList();
            if (!(this.FindMethod(lambda.Type, "Invoke", false, args) is FindMethodsResult.SimpleResult))
            {
                throw this.ParseError(errorPos, ExpressionParsersResources.ArgsIncompatibleWithLambda);
            }
            return Expression.Invoke(lambda, args);
        }

        Expression ParseTypeAccess(Type type)
        {
            int errorPos = this._token.pos;
            this.NextToken();
            if (this._token.id == TokenId.Question)
            {
                if (!type.IsValueType || IsNullableType(type))
                    throw this.ParseError(errorPos, ExpressionParsersResources.TypeHasNoNullableForm, GetTypeName(type));
                type = typeof(Nullable<>).MakeGenericType(type);
                this.NextToken();
            }
            if (this._token.id == TokenId.OpenParen)
            {
                Expression[] args = this.ParseArgumentList();
                var result = this.FindBestMethod(type.GetConstructors(), args);
                if (result is FindMethodsResult.NoneResult)
                {
                    if (args.Length == 1)
                    {
                        return this.GenerateConversion(args[0], type, errorPos);
                    }
                    throw this.ParseError(errorPos, ExpressionParsersResources.NoMatchingConstructor, GetTypeName(type));
                }
                if (result is FindMethodsResult.SimpleResult)
                {
                    return Expression.New((ConstructorInfo)((MethodBase)((FindMethodsResult.SimpleResult)result).MethodInfo), args);
                }
                throw this.ParseError(errorPos, ExpressionParsersResources.AmbiguousConstructorInvocation, GetTypeName(type));
            }
            this.ValidateToken(TokenId.Dot, ExpressionParsersResources.DotOrOpenParenExpected);
            this.NextToken();
            return this.ParseMemberAccess(type, null);
        }

        Expression GenerateConversion(Expression expr, Type type, int errorPos)
        {
            Type exprType = expr.Type;
            if (exprType == type) return expr;
            if (exprType.IsValueType && type.IsValueType)
            {
                if ((IsNullableType(exprType) || IsNullableType(type)) &&
                    GetNonNullableType(exprType) == GetNonNullableType(type))
                    return Expression.Convert(expr, type);
                if ((IsNumericType(exprType) || IsEnumType(exprType)) &&
                    (IsNumericType(type)) || IsEnumType(type))
                    return Expression.ConvertChecked(expr, type);
            }
            if (exprType.IsAssignableFrom(type) || type.IsAssignableFrom(exprType) ||
                exprType.IsInterface || type.IsInterface)
                return Expression.Convert(expr, type);
            throw this.ParseError(errorPos, ExpressionParsersResources.CannotConvertValue,
                                  GetTypeName(exprType), GetTypeName(type));
        }

        Expression ParseMemberAccess(Type type, Expression instance)
        {
            if (instance != null)
            {
                type = instance.Type;
            }
            var errorPos = this._token.pos;
            var id = this.GetIdentifier();
            this.NextToken();
            if (this._token.id == TokenId.OpenParen)
            {
                #region original
                //if (instance != null && type != typeof(string))
                //{
                //    Type enumerableType = FindGenericType(typeof(IEnumerable<>), type);
                //    if (enumerableType != null)
                //    {yt? uthjbb
                //        Type elementType = enumerableType.GetGenericArguments()[0];
                //        return ParseAggregate(instance, elementType, id, errorPos);
                //    }
                //}
                #endregion

                var args = this.ParseArgumentList();

                var findMethodsResult = this.FindMethod(type, id, instance == null, args);

                if (findMethodsResult is FindMethodsResult.SimpleResult)
                {
                    var simple = ((FindMethodsResult.SimpleResult)findMethodsResult);
                    var method = (MethodInfo)simple.MethodInfo;

                    //if (!IsPredefinedType(method.DeclaringType))
                    //{
                    //    throw ParseError(errorPos, ExpressionParsersResources.MethodsAreInaccessible, GetTypeName(method.DeclaringType));
                    //}
                    //if (method.ReturnType == typeof(void))
                    //{
                    //    throw ParseError(errorPos, ExpressionParsersResources.MethodIsVoid, id, GetTypeName(method.DeclaringType));
                    //}
                    return Expression.Call(instance, (MethodInfo)method, args);
                }
                if (findMethodsResult is FindMethodsResult.ExtensionResult)
                {
                    var multy = (FindMethodsResult.ExtensionResult)findMethodsResult;
                    var types = new Type[0];
                    if (multy.MethodInfo.IsGenericMethod)
                    {
                        types = new Type[] { multy.GenericType };
                    }

                    return Expression.Call(
                                           multy.MethodInfo.DeclaringType,
                                           multy.MethodInfo.Name,
                                           types,
                                           new[] { instance }.Concat(args).ToArray());
                }

                if (findMethodsResult is FindMethodsResult.NoneResult)
                {
                    var baseError = this.ParseError(errorPos, ExpressionParsersResources.NoApplicableMethod, id, GetTypeName(type));

                    throw type.ToMissingParsingMethodException(baseError.Message, errorPos, id, args.Select(expr => expr.Type));
                }
                throw this.ParseError(errorPos, ExpressionParsersResources.AmbiguousMethodInvocation, id, GetTypeName(type));
            }
            else
            {
                var member = this.FindPropertyOrField(type, id, instance == null);

                if (member == null)
                {
                    throw this.ParseError(errorPos, ExpressionParsersResources.UnknownPropertyOrField, id, GetTypeName(type));
                }
                return member is PropertyInfo ?
                               Expression.Property(instance, (PropertyInfo)member) :
                               Expression.Field(instance, (FieldInfo)member);
            }
        }

        static Type FindGenericType(Type generic, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericTypeImplementation(generic)) return type;
                if (generic.IsInterface)
                {
                    foreach (Type intfType in type.GetInterfaces())
                    {
                        Type found = FindGenericType(generic, intfType);
                        if (found != null) return found;
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        Expression[] ParseArgumentList()
        {
            this.ValidateToken(TokenId.OpenParen, ExpressionParsersResources.OpenParenExpected);
            this.NextToken();
            Expression[] args = this._token.id != TokenId.CloseParen ? this.ParseArguments() : new Expression[0];
            this.ValidateToken(TokenId.CloseParen, ExpressionParsersResources.CloseParenOrCommaExpected);
            this.NextToken();
            return args;
        }

        Expression[] ParseArguments()
        {
            List<Expression> argList = new List<Expression>();
            while (true)
            {
                argList.Add(this.ParseExpression());
                if (this._token.id != TokenId.Comma) break;
                this.NextToken();
            }
            return argList.ToArray();
        }

        Expression ParseElementAccess(Expression expr)
        {
            int errorPos = this._token.pos;
            this.ValidateToken(TokenId.OpenBracket, ExpressionParsersResources.OpenParenExpected);
            this.NextToken();
            Expression[] args = this.ParseArguments();
            this.ValidateToken(TokenId.CloseBracket, ExpressionParsersResources.CloseBracketOrCommaExpected);
            this.NextToken();
            if (expr.Type.IsArray)
            {
                if (expr.Type.GetArrayRank() != 1 || args.Length != 1)
                    throw this.ParseError(errorPos, ExpressionParsersResources.CannotIndexMultiDimArray);
                Expression index = this.PromoteExpression(args[0], typeof(int), true);
                if (index == null)
                    throw this.ParseError(errorPos, ExpressionParsersResources.InvalidIndex);
                return Expression.ArrayIndex(expr, index);
            }
            else
            {
                var result = this.FindIndexer(expr.Type, args);
                if (result is FindMethodsResult.NoneResult)
                {
                    throw this.ParseError(errorPos, ExpressionParsersResources.NoApplicableIndexer,
                                          GetTypeName(expr.Type));
                }
                if (result is FindMethodsResult.SimpleResult)
                {
                    var simple = ((FindMethodsResult.SimpleResult)result);
                    return Expression.Call(expr, (MethodInfo)simple.MethodInfo, args);
                }
                throw this.ParseError(errorPos, ExpressionParsersResources.AmbiguousIndexerInvocation, GetTypeName(expr.Type));
            }
        }

        static bool IsPredefinedType(Type type)
        {
            foreach (Type t in predefinedTypes) if (t == type) return true;
            return false;
        }

        static bool IsNullableType(Type type)
        {
            return type.IsGenericTypeImplementation(typeof(Nullable<>));
        }

        static Type GetNonNullableType(Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        static string GetTypeName(Type type)
        {
            Type baseType = GetNonNullableType(type);
            string s = baseType.Name;
            if (type != baseType) s += '?';
            return s;
        }

        static bool IsNumericType(Type type)
        {
            return GetNumericTypeKind(type) != 0;
        }

        static bool IsSignedIntegralType(Type type)
        {
            return GetNumericTypeKind(type) == 2;
        }

        static bool IsUnsignedIntegralType(Type type)
        {
            return GetNumericTypeKind(type) == 3;
        }

        static int GetNumericTypeKind(Type type)
        {
            type = GetNonNullableType(type);
            if (type.IsEnum) return 0;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return 1;
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return 2;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return 3;
                default:
                    return 0;
            }
        }

        static bool IsEnumType(Type type)
        {
            return GetNonNullableType(type).IsEnum;
        }

        void CheckAndPromoteOperand(Type signatures, string opName, ref Expression expr, int errorPos)
        {
            var args = new Expression[] { expr };
            if (!(this.FindMethod(signatures, "F", false, args) is FindMethodsResult.SimpleResult))
            {
                throw this.ParseError(errorPos, ExpressionParsersResources.IncompatibleOperand, opName, GetTypeName(args[0].Type));
            }
            expr = args[0];
        }

        void CheckAndPromoteOperands(Type signatures, string opName, ref Expression left, ref Expression right, int errorPos)
        {
            var args = new Expression[] { left, right };
            if (!(this.FindMethod(signatures, "F", false, args) is FindMethodsResult.SimpleResult))
            {
                throw this.IncompatibleOperandsError(opName, left, right, errorPos);
            }
            left = args[0];
            right = args[1];
        }

        Exception IncompatibleOperandsError(string opName, Expression left, Expression right, int pos)
        {
            return this.ParseError(pos, ExpressionParsersResources.IncompatibleOperands,
                                   opName, GetTypeName(left.Type), GetTypeName(right.Type));
        }

        MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
                                 (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type t in SelfAndBaseTypes(type))
            {
                MemberInfo[] members = t.FindMembers(MemberTypes.Property | MemberTypes.Field,
                                                     flags, Type.FilterNameIgnoreCase, memberName);
                if (members.Length != 0) return members[0];
            }
            return null;
        }

        FindMethodsResult FindMethod(Type type, string methodName, bool staticAccess, Expression[] args)
        {
            var flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);

            foreach (var t in SelfAndBaseTypes(type))
            {
                var members = t.FindMembers(MemberTypes.Method, flags, Type.FilterNameIgnoreCase, methodName);
                var result = this.FindBestMethod(members.Cast<MethodBase>(), args);

                if (!(result is FindMethodsResult.NoneResult))
                {
                    return result;
                }
            }
            var isEnumerable = type.GetInterfaces().Contains(typeof(IEnumerable));
            if (isEnumerable)
            {
                var findMethods = typeof(Enumerable).FindMembers(
                                                                 MemberTypes.Method,
                                                                 BindingFlags.Public | BindingFlags.Static,
                                                                 Type.FilterNameIgnoreCase, methodName);

                var method = findMethods.Cast<MethodInfo>().FirstOrDefault(z => z.GetParameters().Count() == args.Count() + 1);


                var genericType = FindGenericType(typeof(IEnumerable<>), type);


                if (null != method)
                {
                    return FindMethodsResult.CreateExtension(typeof(Enumerable), method, genericType.GetGenericArguments().FirstOrDefault());
                }
            }

            return FindMethodsResult.CreateNone();
        }

        FindMethodsResult FindIndexer(Type type, Expression[] args)
        {
            foreach (Type t in SelfAndBaseTypes(type))
            {
                var members = t.GetDefaultMembers();

                if (members.Length != 0)
                {
                    var methods = members.
                                  OfType<PropertyInfo>().
                                  Select(p => (MethodBase)p.GetGetMethod()).
                                  Where(m => m != null);

                    var result = this.FindBestMethod(methods, args);

                    if (!(result is FindMethodsResult.NoneResult))
                    {
                        return result;
                    }
                }
            }
            return FindMethodsResult.CreateNone();
        }

        static IEnumerable<Type> SelfAndBaseTypes(Type type)
        {
            if (type.IsInterface)
            {
                List<Type> types = new List<Type>();
                AddInterface(types, type);
                return types;
            }
            return SelfAndBaseClasses(type);
        }

        static IEnumerable<Type> SelfAndBaseClasses(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

        static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                foreach (Type t in type.GetInterfaces()) AddInterface(types, t);
            }
        }

        class MethodData
        {
            public MethodBase MethodBase;
            public ParameterInfo[] Parameters;
            public Expression[] Args;
        }

        FindMethodsResult FindBestMethod(IEnumerable<MethodBase> methods, Expression[] args)
        {
            var applicable = methods.
                             Select(m => new MethodData { MethodBase = m, Parameters = m.GetParameters() }).
                             Where(m => this.IsApplicable(m, args)).
                             ToArray();
            if (applicable.Length > 1)
            {
                applicable = applicable.
                             Where(m => applicable.All(n => m == n || IsBetterThan(args, m, n))).
                             ToArray();
            }
            if (1 == applicable.Length)
            {
                var md = applicable[0];
                for (var i = 0; i < args.Length; i++)
                {
                    args[i] = md.Args[i];
                }
                return FindMethodsResult.CreateSimple((MethodInfo)md.MethodBase);
            }
            if (0 == applicable.Length)
            {
                return FindMethodsResult.CreateNone();
            }
            return FindMethodsResult.CreateMulty(applicable.Length);
        }

        bool IsApplicable(MethodData method, Expression[] args)
        {
            if (method.Parameters.Length != args.Length) return false;
            var promotedArgs = new Expression[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                ParameterInfo pi = method.Parameters[i];
                if (pi.IsOut) return false;
                Expression promoted = this.PromoteExpression(args[i], pi.ParameterType, false);
                if (promoted == null) return false;
                promotedArgs[i] = promoted;
            }
            method.Args = promotedArgs;
            return true;
        }

        Expression PromoteExpression(Expression expr, Type type, bool exact)
        {
            if (expr.Type == type) return expr;
            if (expr is ConstantExpression)
            {
                ConstantExpression ce = (ConstantExpression)expr;
                if (ce == nullLiteral)
                {
                    if (!type.IsValueType || IsNullableType(type))
                        return Expression.Constant(null, type);
                }
                else
                {
                    string text;
                    if (this.literals.TryGetValue(ce, out text))
                    {
                        Type target = GetNonNullableType(type);
                        object value = null;
                        switch (Type.GetTypeCode(ce.Type))
                        {
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                                value = ParseNumber(text, target);
                                break;
                            case TypeCode.Double:
                                if (target == typeof(decimal)) value = ParseNumber(text, target);
                                break;
                            case TypeCode.String:
                                value = ParseEnum(text, target);
                                break;
                        }
                        if (value != null)
                            return Expression.Constant(value, type);
                    }
                }
            }
            if (IsCompatibleWith(expr.Type, type))
            {
                if (type.IsValueType || exact) return Expression.Convert(expr, type);
                return expr;
            }
            return null;
        }

        static object ParseNumber(string text, Type type)
        {
            switch (Type.GetTypeCode(GetNonNullableType(type)))
            {
                case TypeCode.SByte:
                    sbyte sb;
                    if (sbyte.TryParse(text, out sb)) return sb;
                    break;
                case TypeCode.Byte:
                    byte b;
                    if (byte.TryParse(text, out b)) return b;
                    break;
                case TypeCode.Int16:
                    short s;
                    if (short.TryParse(text, out s)) return s;
                    break;
                case TypeCode.UInt16:
                    ushort us;
                    if (ushort.TryParse(text, out us)) return us;
                    break;
                case TypeCode.Int32:
                    int i;
                    if (int.TryParse(text, out i)) return i;
                    break;
                case TypeCode.UInt32:
                    uint ui;
                    if (uint.TryParse(text, out ui)) return ui;
                    break;
                case TypeCode.Int64:
                    long l;
                    if (long.TryParse(text, out l)) return l;
                    break;
                case TypeCode.UInt64:
                    ulong ul;
                    if (ulong.TryParse(text, out ul)) return ul;
                    break;
                case TypeCode.Single:
                    float f;
                    if (float.TryParse(text, out f)) return f;
                    break;
                case TypeCode.Double:
                    double d;
                    if (double.TryParse(text, out d)) return d;
                    break;
                case TypeCode.Decimal:
                    decimal e;
                    if (decimal.TryParse(text, out e)) return e;
                    break;
            }
            return null;
        }

        static object ParseEnum(string name, Type type)
        {
            if (type.IsEnum)
            {
                MemberInfo[] memberInfos = type.FindMembers(MemberTypes.Field,
                                                            BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static,
                                                            Type.FilterNameIgnoreCase, name);
                if (memberInfos.Length != 0) return ((FieldInfo)memberInfos[0]).GetValue(null);
            }
            return null;
        }

        static bool IsCompatibleWith(Type source, Type target)
        {
            if (source == target) return true;
            if (!target.IsValueType) return target.IsAssignableFrom(source);
            Type st = GetNonNullableType(source);
            Type tt = GetNonNullableType(target);
            if (st != source && tt == target) return false;
            TypeCode sc = st.IsEnum ? TypeCode.Object : Type.GetTypeCode(st);
            TypeCode tc = tt.IsEnum ? TypeCode.Object : Type.GetTypeCode(tt);
            switch (sc)
            {
                case TypeCode.SByte:
                    switch (tc)
                    {
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Byte:
                    switch (tc)
                    {
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int16:
                    switch (tc)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt16:
                    switch (tc)
                    {
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int32:
                    switch (tc)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt32:
                    switch (tc)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int64:
                    switch (tc)
                    {
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt64:
                    switch (tc)
                    {
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Single:
                    switch (tc)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                    }
                    break;
                default:
                    if (st == tt) return true;
                    break;
            }
            return false;
        }

        static bool IsBetterThan(Expression[] args, MethodData m1, MethodData m2)
        {
            bool better = false;
            for (int i = 0; i < args.Length; i++)
            {
                int c = CompareConversions(args[i].Type,
                                           m1.Parameters[i].ParameterType,
                                           m2.Parameters[i].ParameterType);
                if (c < 0)
                {
                    return false;
                }
                if (c > 0)
                {
                    better = true;
                }
            }
            return better;
        }

        // Return 1 if s -> t1 is a better conversion than s -> t2
        // Return -1 if s -> t2 is a better conversion than s -> t1
        // Return 0 if neither conversion is better
        static int CompareConversions(Type s, Type t1, Type t2)
        {
            if (t1 == t2) return 0;
            if (s == t1) return 1;
            if (s == t2) return -1;
            bool t1t2 = IsCompatibleWith(t1, t2);
            bool t2t1 = IsCompatibleWith(t2, t1);
            if (t1t2 && !t2t1) return 1;
            if (t2t1 && !t1t2) return -1;
            if (IsSignedIntegralType(t1) && IsUnsignedIntegralType(t2)) return 1;
            if (IsSignedIntegralType(t2) && IsUnsignedIntegralType(t1)) return -1;
            return 0;
        }

        Expression GenerateEqual(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }

        Expression GenerateNotEqual(Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }

        Expression GenerateGreaterThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThan(this.GenerateStaticMethodCall("Compare", left, right),
                                              Expression.Constant(0)
                                             );
            }
            return Expression.GreaterThan(left, right);
        }

        Expression GenerateGreaterThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThanOrEqual(this.GenerateStaticMethodCall("Compare", left, right),
                                                     Expression.Constant(0)
                                                    );
            }
            return Expression.GreaterThanOrEqual(left, right);
        }

        Expression GenerateLessThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThan(this.GenerateStaticMethodCall("Compare", left, right),
                                           Expression.Constant(0)
                                          );
            }
            return Expression.LessThan(left, right);
        }

        Expression GenerateLessThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThanOrEqual(this.GenerateStaticMethodCall("Compare", left, right),
                                                  Expression.Constant(0)
                                                 );
            }
            return Expression.LessThanOrEqual(left, right);
        }

        Expression GenerateAdd(Expression left, Expression right)
        {
            if (left.Type == typeof(string) && right.Type == typeof(string))
            {
                return this.GenerateStaticMethodCall("Concat", left, right);
            }
            return Expression.Add(left, right);
        }

        Expression GenerateSubtract(Expression left, Expression right)
        {
            return Expression.Subtract(left, right);
        }

        Expression GenerateStringConcat(Expression left, Expression right)
        {
            return Expression.Call(
                                   null,
                                   typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) }),
                                   new[] { left, right });
        }

        MethodInfo GetStaticMethod(string methodName, Expression left, Expression right)
        {
            return left.Type.GetMethod(methodName, new[] { left.Type, right.Type });
        }

        Expression GenerateStaticMethodCall(string methodName, Expression left, Expression right)
        {
            return Expression.Call(null, this.GetStaticMethod(methodName, left, right), new[] { left, right });
        }

        void SetTextPos(int pos)
        {
            this._textPos = pos;
            this._currentChar = this._textPos < this._textLen ? this._text[this._textPos] : '\0';
        }

        void NextChar()
        {
            if (this._textPos < this._textLen) this._textPos++;
            this._currentChar = this._textPos < this._textLen ? this._text[this._textPos] : '\0';
        }

        void NextToken()
        {
            while (char.IsWhiteSpace(this._currentChar))
            {
                this.NextChar();
            }
            TokenId t;
            int tokenPos = this._textPos;
            switch (this._currentChar)
            {
                case '!':
                    this.NextChar();
                    if (this._currentChar == '=')
                    {
                        this.NextChar();
                        t = TokenId.ExclamationEqual;
                    }
                    else
                    {
                        t = TokenId.Exclamation;
                    }
                    break;
                case '%':
                    this.NextChar();
                    t = TokenId.Percent;
                    break;
                case '&':
                    this.NextChar();
                    if (this._currentChar == '&')
                    {
                        this.NextChar();
                        t = TokenId.DoubleAmphersand;
                    }
                    else
                    {
                        t = TokenId.Amphersand;
                    }
                    break;
                case '(':
                    this.NextChar();
                    t = TokenId.OpenParen;
                    break;
                case ')':
                    this.NextChar();
                    t = TokenId.CloseParen;
                    break;
                case '*':
                    this.NextChar();
                    t = TokenId.Asterisk;
                    break;
                case '+':
                    this.NextChar();
                    t = TokenId.Plus;
                    break;
                case ',':
                    this.NextChar();
                    t = TokenId.Comma;
                    break;
                case '-':
                    this.NextChar();
                    t = TokenId.Minus;
                    break;
                case '.':
                    this.NextChar();
                    t = TokenId.Dot;
                    break;
                case '/':
                    this.NextChar();
                    t = TokenId.Slash;
                    break;
                case ':':
                    this.NextChar();
                    t = TokenId.Colon;
                    break;
                case '<':
                    this.NextChar();
                    if (this._currentChar == '=')
                    {
                        this.NextChar();
                        t = TokenId.LessThanEqual;
                    }
                    else if (this._currentChar == '>')
                    {
                        this.NextChar();
                        t = TokenId.LessGreater;
                    }
                    else
                    {
                        t = TokenId.LessThan;
                    }
                    break;
                case '=':
                    this.NextChar();
                    if (this._currentChar == '=')
                    {
                        this.NextChar();
                        t = TokenId.DoubleEqual;
                    }
                    else
                    {
                        t = TokenId.Equal;
                    }
                    break;
                case '>':
                    this.NextChar();
                    if (this._currentChar == '=')
                    {
                        this.NextChar();
                        t = TokenId.GreaterThanEqual;
                    }
                    else
                    {
                        t = TokenId.GreaterThan;
                    }
                    break;
                case '?':
                    this.NextChar();
                    t = TokenId.Question;
                    break;
                case '[':
                    this.NextChar();
                    t = TokenId.OpenBracket;
                    break;
                case ']':
                    this.NextChar();
                    t = TokenId.CloseBracket;
                    break;
                case '|':
                    this.NextChar();
                    if (this._currentChar == '|')
                    {
                        this.NextChar();
                        t = TokenId.DoubleBar;
                    }
                    else
                    {
                        t = TokenId.Bar;
                    }
                    break;
                case '"':
                case '\'':
                    char quote = this._currentChar;
                    do
                    {
                        this.NextChar();
                        while (this._textPos < this._textLen && this._currentChar != quote)
                        {
                            this.NextChar();
                        }
                        if (this._textPos == this._textLen)
                        {
                            throw this.ParseError(this._textPos, ExpressionParsersResources.UnterminatedStringLiteral);
                        }
                        this.NextChar();
                    } while (this._currentChar == quote);
                    t = TokenId.StringLiteral;
                    break;
                default:
                    if (char.IsLetter(this._currentChar) || this._currentChar == '@' || this._currentChar == '_')
                    {
                        do
                        {
                            this.NextChar();
                        } while (char.IsLetterOrDigit(this._currentChar) || this._currentChar == '_');
                        t = TokenId.Identifier;
                        break;
                    }
                    if (char.IsDigit(this._currentChar))
                    {
                        t = TokenId.IntegerLiteral;
                        do
                        {
                            this.NextChar();
                        } while (char.IsDigit(this._currentChar));
                        if (this._currentChar == '.')
                        {
                            t = TokenId.RealLiteral;
                            this.NextChar();
                            this.ValidateDigit();
                            do
                            {
                                this.NextChar();
                            } while (char.IsDigit(this._currentChar));
                        }
                        if (this._currentChar == 'E' || this._currentChar == 'e')
                        {
                            t = TokenId.RealLiteral;
                            this.NextChar();
                            if (this._currentChar == '+' || this._currentChar == '-') this.NextChar();
                            this.ValidateDigit();
                            do
                            {
                                this.NextChar();
                            } while (char.IsDigit(this._currentChar));
                        }
                        if (this._currentChar == 'F' || this._currentChar == 'f') this.NextChar();
                        break;
                    }
                    if (this._textPos == this._textLen)
                    {
                        t = TokenId.End;
                        break;
                    }
                    throw this.ParseError(this._textPos, ExpressionParsersResources.InvalidCharacter, this._currentChar);
            }
            this._token.id = t;
            this._token.text = this._text.Substring(tokenPos, this._textPos - tokenPos);
            this._token.pos = tokenPos;
        }

        bool TokenIdentifierIs(string id)
        {
            return this._token.id == TokenId.Identifier && string.Equals(id, this._token.text, StringComparison.OrdinalIgnoreCase);
        }

        string GetIdentifier()
        {
            this.ValidateToken(TokenId.Identifier, ExpressionParsersResources.IdentifierExpected);
            string id = this._token.text;
            if (id.Length > 1 && id[0] == '@')
            {
                id = id.Substring(1);
            }
            return id;
        }

        void ValidateDigit()
        {
            if (!char.IsDigit(this._currentChar)) throw this.ParseError(this._textPos, ExpressionParsersResources.DigitExpected);
        }

        void ValidateToken(TokenId t, string errorMessage)
        {
            if (this._token.id != t)
            {
                throw this.ParseError(errorMessage);
            }
        }

        void ValidateToken(TokenId t)
        {
            if (this._token.id != t)
            {
                throw this.ParseError(ExpressionParsersResources.SyntaxError);
            }
        }

        Exception ParseError(string format, params object[] args)
        {
            return this.ParseError(this._token.pos, format, args);
        }

        Exception ParseError(int pos, string format, params object[] args)
        {
            return new ParseException(string.Format(CultureInfo.CurrentCulture, format, args), pos);
        }



        static Dictionary<string, object> CreateKeywords()
        {
            var d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                            {"true", trueLiteral},
                            {"false", falseLiteral},
                            {"null", nullLiteral},
                            {keywordIt, keywordIt},
                            {keywordIif, keywordIif},
                            {keywordNew, keywordNew}
                    };
            foreach (var type in predefinedTypes)
            {
                d.Add(type.Name, type);
            }
            return d;
        }
    }

}
