using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Framework.Core;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// TypeScript code generator
    /// </summary>
    public sealed class TypeScriptCodeGenerator : CodeCompiler
    {
        private const int MaxLineLength = 80;
        private const GeneratorSupport LanguageSupport = GeneratorSupport.DeclareEnums;
        private static readonly CodeTypeReference VoidType = typeof(void).ToTypeReference();

        private static readonly Hashtable Keywords;

        private static readonly Regex OutputReg =
            new Regex(
                @"(([^(]+)(\(([0-9]+),([0-9]+)\))[ \t]*:[ \t]+)?(fatal )?(error|warning)[ \t]+([A-Z]+[0-9]+)[ \t]*:[ \t]*(.*)");

        private readonly string defaultNamespace;

        private bool forLoopHack = false;
        private bool isArgumentList = true;
        private string mainClassName = null;
        private string mainMethodName = null;

        static TypeScriptCodeGenerator()
        {
            // build the keywords hashtable
            Keywords = new Hashtable(150);
            var obj = new object();

            // a
            Keywords["abstract"] = obj;
            Keywords["assert"] = obj;

            // b
            Keywords["boolean"] = obj;
            Keywords["break"] = obj;
            Keywords["byte"] = obj;

            // c
            Keywords["case"] = obj;
            Keywords["catch"] = obj;
            Keywords["char"] = obj;
            Keywords["class"] = obj;
            Keywords["const"] = obj;
            Keywords["continue"] = obj;

            // d
            Keywords["debugger"] = obj;
            Keywords["decimal"] = obj;
            Keywords["default"] = obj;
            Keywords["delete"] = obj;
            Keywords["do"] = obj;
            Keywords["double"] = obj;

            // e
            Keywords["else"] = obj;
            Keywords["ensure"] = obj;
            Keywords["enum"] = obj;
            Keywords["event"] = obj;
            Keywords["export"] = obj;
            Keywords["extends"] = obj;

            // f
            Keywords["false"] = obj;
            Keywords["final"] = obj;
            Keywords["finally"] = obj;
            Keywords["float"] = obj;
            Keywords["for"] = obj;
            Keywords["function"] = obj;

            // g
            Keywords["get"] = obj;
            Keywords["goto"] = obj;

            // i
            Keywords["if"] = obj;
            Keywords["implements"] = obj;
            Keywords["import"] = obj;
            Keywords["in"] = obj;
            Keywords["instanceof"] = obj;
            Keywords["int"] = obj;
            Keywords["invariant"] = obj;
            Keywords["interface"] = obj;
            Keywords["internal"] = obj;

            // l
            Keywords["long"] = obj;

            // n
            Keywords["namespace"] = obj;
            Keywords["native"] = obj;
            Keywords["new"] = obj;
            Keywords["null"] = obj;

            // p
            Keywords["package"] = obj;
            Keywords["private"] = obj;
            Keywords["protected"] = obj;
            Keywords["public"] = obj;

            // r
            Keywords["require"] = obj;
            Keywords["return"] = obj;

            // s
            Keywords["sbyte"] = obj;
            Keywords["scope"] = obj;
            Keywords["set"] = obj;
            Keywords["short"] = obj;
            Keywords["static"] = obj;
            Keywords["super"] = obj;
            Keywords["switch"] = obj;
            Keywords["synchronized"] = obj;

            // t
            Keywords["this"] = obj;
            Keywords["throw"] = obj;
            Keywords["throws"] = obj;
            Keywords["transient"] = obj;
            Keywords["true"] = obj;
            Keywords["try"] = obj;
            Keywords["typeof"] = obj;

            // u
            Keywords["use"] = obj;
            Keywords["uint"] = obj;
            Keywords["ulong"] = obj;
            Keywords["ushort"] = obj;

            // v
            Keywords["var"] = obj;
            Keywords["void"] = obj;
            Keywords["volatile"] = obj;

            // w
            Keywords["while"] = obj;
            Keywords["with"] = obj;
        }

        public TypeScriptCodeGenerator(string defaultNamespace)
        {
            this.defaultNamespace = defaultNamespace;
        }

        /// <summary>
        ///     Gets the name of the compiler executable.
        /// </summary>
        protected override string CompilerName => "tsc.exe";

        /// <summary>
        ///     Gets the file extension to use for source files.
        /// </summary>
        protected override string FileExtension => ".ts";

        protected override string NullToken => "null";

        public static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
            {
                return str;
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        protected override string CmdArgsFromParameters(CompilerParameters options)
        {
            return string.Empty;
        }

        protected override string CreateEscapedIdentifier(string name)
        {
            if (this.IsKeyword(name))
            {
                return name;
            }

            return name;
        }

        protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
            this.OutputIdentifier(e.ParameterName);
        }

        protected override string CreateValidIdentifier(string name)
        {
            if (this.IsKeyword(name))
            {
                return "$" + name;
            }

            return name;
        }

        protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
        {
            CodeExpressionCollection init = e.Initializers;
            if (init.Count > 0)
            {
                this.Output.Write("[");
                this.Indent++;
                this.OutputExpressionList(init);
                this.Indent--;
                this.Output.Write("]");
            }
            else
            {
                var s = string.Empty;
                this.Output.Write(s);
                this.Output.Write(this.GetTypeOutput(e.CreateType));
                this.Output.Write("(");
                if (e.SizeExpression != null)
                {
                    this.GenerateExpression(e.SizeExpression);
                }
                else if (e.Size != 0)
                {
                    this.Output.Write(e.Size.ToString(CultureInfo.InvariantCulture));
                }

                this.Output.Write(")");
            }
        }

        protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
        {
            this.GenerateExpression(e.TargetObject);
            this.Output.Write("[");
            bool first = true;
            foreach (CodeExpression exp in e.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    this.Output.Write(", ");
                }

                this.GenerateExpression(exp);
            }

            this.Output.Write("]");
        }

        protected override void GenerateAssignStatement(CodeAssignStatement e)
        {
                this.GenerateExpression(e.Left);
                this.Output.Write(" = ");
                this.GenerateExpression(e.Right);
                if (!this.forLoopHack)
                {
                    this.Output.WriteLine(";");
                }
        }

        // Generates code for the specified CodeDom based attribute block end representation.
        // 'attributes' indicates the attribute block end to generate code for.
        protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
        }

        // Generates code for the specified CodeDom based attribute block start representation.
        // 'attributes' indicates the attribute block to generate code for.
        protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
        }

        // Generates code for the specified CodeDom based base reference expression
        // representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
        {
            this.Output.Write("super");
        }

        // Generates code for the specified CodeDom based cast expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateCastExpression(CodeCastExpression e)
        {
            this.OutputType(e.TargetType);
            this.Output.Write("(");
            this.GenerateExpression(e.Expression);
            this.Output.Write(")");
        }

        protected override void GenerateComment(CodeComment e)
        {
            string commentText = e.Text;
            var b = new StringBuilder(commentText.Length * 2);
            string commentPrefix = e.DocComment ? "///" : "//";

            // escape the comment text
            b.Append(commentPrefix);
            for (int i = 0; i < commentText.Length; i++)
            {
                switch (commentText[i])
                {
                    case '@':

                        // suppress '@' to prevent compiler directives in comments
                        break;
                    case '\r':
                        if (i < commentText.Length - 1 && commentText[i + 1] == '\n')
                        {
                            b.Append("\r\n" + commentPrefix);
                        }
                        else
                        {
                            b.Append("\r" + commentPrefix);
                        }

                        break;
                    case '\n':
                        b.Append("\n" + commentPrefix);
                        break;
                    default:
                        b.Append(commentText[i]);
                        break;
                }
            }

            this.Output.WriteLine(b.ToString());
        }

        protected override void GenerateCompileUnitStart(CodeCompileUnit e)
        {
            this.Output.WriteLine("// ------------------------------------------------------------------------------");
            this.Output.WriteLine("/// <auto-generated>");
            this.Output.WriteLine("/// This code was generated by a tool.");
            this.Output.WriteLine("///");
            this.Output.WriteLine("/// Changes to this file may cause incorrect behavior and will be lost if");
            this.Output.WriteLine("/// the code is regenerated.");
            this.Output.WriteLine("/// </auto-generated>");
            this.Output.WriteLine("// ------------------------------------------------------------------------------");

            // Disabling built-in tslint rules
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("// tslint:disable");
            this.Output.WriteLine("/* eslint-disable */");

            // Assembly attributes must be placed at the top level.
            if (e.AssemblyCustomAttributes.Count > 0)
            {
                this.GenerateAssemblyAttributes(e.AssemblyCustomAttributes);
                this.Output.WriteLine(string.Empty);
            }
        }

        // Generates code for the specified CodeDom based if statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateConditionStatement(CodeConditionStatement e)
        {
            this.Output.Write("if (");

            // Indent within the condition, in case they need to go on multiple lines.
            this.Indent += 2;
            this.GenerateExpression(e.Condition);
            this.Indent -= 2;
            this.Output.Write(")");
            this.OutputStartingBrace();
            this.Indent++;
            this.GenerateStatements(e.TrueStatements);
            this.Indent--;

            if (e.FalseStatements.Count > 0)
            {
                this.Output.Write("}");
                if (this.Options.ElseOnClosing)
                {
                    this.Output.Write(" ");
                }
                else
                {
                    this.Output.WriteLine(string.Empty);
                }

                this.Output.Write("else");
                this.OutputStartingBrace();
                this.Indent++;
                this.GenerateStatements(e.FalseStatements);
                this.Indent--;
            }

            this.Output.WriteLine("}");
        }

        // Generates code for the specified CodeDom based constructor representation.
        // 'e' indicates the constructor to generate code for.
        protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
        {
            if (!(this.IsCurrentClass || this.IsCurrentStruct))
            {
                return;
            }

            var codeConstructors = c.Members.Cast<object>().Where(m => m is CodeConstructor).Cast<CodeConstructor>().ToList();
            var codeConstructorWithMaxParam = codeConstructors.OrderBy(x => x.Parameters.Count).Last();
            if (codeConstructors.Count > 1 && e != codeConstructorWithMaxParam)
            {
                this.Output.Write("constructor ");
                this.Output.Write("(");
                this.OutputParameters(e.Parameters);
                this.Output.Write(")");
                this.Output.WriteLine(";");
                return;
            }

            if (e.CustomAttributes.Count > 0)
            {
                this.OutputAttributeDeclarations(e.CustomAttributes);
            }

            this.Output.Write("constructor ");

            this.Output.Write("(");
            this.OutputParameters(e.Parameters);
            this.Output.Write(")");

            CodeExpressionCollection baseArgs = e.BaseConstructorArgs;

            this.OutputStartingBrace();
            this.Indent++;

            if (baseArgs.Count > 0)
            {
                this.Output.Write("super(");
                this.OutputExpressionList(baseArgs);
                this.Output.WriteLine(");");
            }
            else
            {
                if (c.BaseTypes.Count > 0 && c.BaseTypes.Cast<CodeTypeReference>().Any(r => r.BaseType.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last().Pipe(s => !s.StartsWith("IIdentity"))))
                {
                    this.Output.WriteLine("super();");
                }
            }

            this.GenerateStatements(e.Statements);
            this.Indent--;
            this.Indent--;
            this.Output.WriteLine();
            this.Indent++;
            this.Output.WriteLine("}");

            this.Indent--;
            this.Output.WriteLine();
            this.Indent++;
        }

        // Generates code for the specified CodeDom based delegate creation expression representation.
        // 'e' indicates the delegate creation expression to generate code for.
        protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
        {
            bool castToDelegate = e.DelegateType != null;
            if (castToDelegate)
            {
                this.OutputType(e.DelegateType);
                this.Output.Write("(");
            }

            this.GenerateExpression(e.TargetObject);
            this.Output.Write(".");
            this.OutputIdentifier(e.MethodName);
            if (castToDelegate)
            {
                this.Output.Write(")");
            }
        }

        // Generates code for the specified CodeDom based delegate invoke expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
        {
            if (e.TargetObject != null)
            {
                this.GenerateExpression(e.TargetObject);
            }

            this.Output.Write("(");
            this.OutputExpressionList(e.Parameters);
            this.Output.Write(")");
        }

        protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
        {
            this.Output.Write("public static ");
            if (e.CustomAttributes.Count > 0)
            {
                this.OutputAttributeDeclarations(e.CustomAttributes);
            }

            this.Output.Write("function Main()");
            this.OutputStartingBrace();
            this.Indent++;
            this.GenerateStatements(e.Statements);
            this.Indent--;
            this.Output.WriteLine("}");
            this.mainClassName = this.CurrentTypeName;
            this.mainMethodName = "Main";
        }

        protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
        {
            if (this.IsCurrentDelegate || this.IsCurrentEnum)
            {
                return;
            }

            if (e.PrivateImplementationType == null)
            {
                this.OutputMemberAccessModifier(e.Attributes);
            }

            this.Output.Write("/*event*/ ");
            string name = e.Name;
            if (e.PrivateImplementationType != null)
            {
                name = e.PrivateImplementationType.BaseType + "." + name;
            }

            this.OutputTypeNamePair(e.Type, name);
            this.Output.WriteLine(";");
        }

        // JScript does not allow ob.foo where foo is an event name. (This should never get
        // called, but we'll put an exception here just in case.)
        // throw new Exception("No event references");
        protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                this.GenerateExpression(e.TargetObject);
                this.Output.Write(".");
            }

            this.OutputIdentifier(e.EventName);
        }

        // Generates code for the specified CodeDom based method invoke statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateExpressionStatement(CodeExpressionStatement e)
        {
            var expression = e.Expression as CodeLambdaExpression;
            if (expression != null)
            {
                this.NormalizeExpression(expression);
            }
            else
            {
                this.GenerateExpression(e.Expression);
            }

            if (!this.forLoopHack)
            {
                this.Output.WriteLine(";");
            }
        }

        // Generates code for the specified CodeDom based member field representation.
        // 'e' indicates the field to generate code for.
        protected override void GenerateField(CodeMemberField e)
        {
            if (this.IsCurrentDelegate || this.IsCurrentInterface)
            {
                throw new Exception("Only methods on interfaces");
            }

            if (this.IsCurrentEnum)
            {
                this.OutputIdentifier(e.Name);

                if (e.InitExpression != null)
                {
                    this.Output.Write(" = ");
                    this.GenerateExpression(e.InitExpression);
                }

                this.Output.WriteLine(",");
            }
            else
            {
                this.OutputMemberAccessModifier(e.Attributes);
                if ((e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)
                {
                    this.Output.Write("static ");
                }

                if ((e.Attributes & MemberAttributes.Const) == MemberAttributes.Const)
                {
                    if ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
                    {
                        this.Output.Write("static ");
                    }
                }

                // Providing field type declaration only if not exists initial expression ( typescript => v.1.8 )
                if (e.InitExpression == null)
                {
                    this.OutputTypeNamePair(e.Type, e.Name);
                }

                if (e.InitExpression != null)
                {
                    this.OutputIdentifier(e.Name);
                    this.Output.Write(" = ");
                    this.GenerateExpression(e.InitExpression);
                }

                this.Output.WriteLine(";");
                this.Indent--;
                this.Output.WriteLine();
                this.Indent++;
            }
        }

        // Generates code for the specified CodeDom based field reference expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                this.GenerateExpression(e.TargetObject);
                this.Output.Write(".");
            }

            this.OutputIdentifier(e.FieldName);
        }

        protected override void GenerateGotoStatement(CodeGotoStatement e)
        {
            throw new Exception("No goto statements");
        }

        // Generates code for the specified CodeDom based indexer expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateIndexerExpression(CodeIndexerExpression e)
        {
            this.GenerateExpression(e.TargetObject);
            this.Output.Write("[");
            bool first = true;
            foreach (CodeExpression exp in e.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    this.Output.Write(", ");
                }

                this.GenerateExpression(exp);
            }

            this.Output.Write("]");
        }

        protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
        {
            this.GenerateExpression(e.Event.TargetObject);
            this.Output.Write(".add_");
            this.Output.Write(e.Event.EventName);
            this.Output.Write("(");
            this.GenerateExpression(e.Listener);
            this.Output.WriteLine(");");
        }

        // Generates code for the specified CodeDom based for loop statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateIterationStatement(CodeIterationStatement e)
        {
            this.forLoopHack = true;
            this.Output.Write("for (");
            this.GenerateStatement(e.InitStatement);
            this.Output.Write("; ");
            this.GenerateExpression(e.TestExpression);
            this.Output.Write("; ");
            this.GenerateStatement(e.IncrementStatement);
            this.Output.Write(")");
            this.OutputStartingBrace();
            this.forLoopHack = false;
            this.Indent++;
            this.GenerateStatements(e.Statements);
            this.Indent--;
            this.Output.WriteLine("}");
        }

        protected override void GenerateLabeledStatement(CodeLabeledStatement e)
        {
            throw new Exception("No goto statements");
        }

        // Generates code for the specified CodeDom based line pragma start representation.
        // 'e' indicates the start position to generate code for.
        protected override void GenerateLinePragmaStart(CodeLinePragma e)
        {
            this.Output.WriteLine(string.Empty);

            // we need to escape "\" to become "\\", however regex requires the
            // _pattern_ to be escaped, therefore the replacement is "\\" --> "\\"...
            // since C# requires \ to be escaped we get the apparently meaningless
            // expression below...
            this.Output.Write(Regex.Replace(e.FileName, "\\\\", "\\\\"));

            this.Output.Write("\";line=");
            this.Output.Write(e.LineNumber.ToString(CultureInfo.InvariantCulture));
            this.Output.WriteLine(")");
        }

        // Generates code for the specified CodeDom based line pragma end representation.
        // 'e' indicates the end position to generate code for.
        protected override void GenerateLinePragmaEnd(CodeLinePragma e)
        {
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("//@set @position(end)");
        }

        // Generates code for the specified CodeDom based member method representation.
        // 'e' indicates the method to generate code for.
        protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
        {
            if (e.Name == "operator ==" || e.Name == "operator !=")
            {
                Debug.WriteLine($"Skip operator '{e.Name}' for '{c.Name}' ");
                return;
            }

            if (e.Name == "Clone" && e.Parameters.Count == 0)
            {
                Debug.WriteLine($"Skip Clone '{e.Name}' for '{c.Name}' ");
                return;
            }

            if (e.PrivateImplementationType != null)
            {
                return;
            }

            if (e.Attributes.HasFlag(MemberAttributes.AccessMask))
            {
                this.Output.Write("function "); /*export */
            }
            else
            {
                if (!this.IsCurrentInterface)
                {
                    if (e.PrivateImplementationType == null)
                    {
                        this.OutputMemberAccessModifier(e.Attributes);
                        this.OutputMemberScopeModifier(e.Attributes);
                    }
                }

                if (e.CustomAttributes.Count > 0)
                {
                    this.OutputAttributeDeclarations(e.CustomAttributes);
                }
            }

            if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
            {
                this.Output.Write(e.PrivateImplementationType.BaseType);
                this.Output.Write(".");
            }

            this.OutputIdentifier(FirstCharacterToLower(e.Name));
            this.Output.Write("(");

            // disable use of '&' in front of ref and out parameters (JScript doesn't allow them to be declared, only used)
            this.isArgumentList = false;
            try
            {
                this.OutputParameters(e.Parameters);
            }
            finally
            {
                this.isArgumentList = true;
            }

            this.Output.Write(")");
            if (e.ReturnType.BaseType.Length > 0 &&
                string.Compare(e.ReturnType.BaseType, typeof(void).FullName, StringComparison.Ordinal) != 0)
            {
                this.Output.Write(": ");
                this.OutputType(e.ReturnType);
            }

            if (!this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
            {
                this.OutputStartingBrace();
                this.Indent++;
                this.GenerateStatements(e.Statements);
                this.Indent--;
                this.Output.WriteLine("}");
            }
            else
            {
                this.Output.WriteLine(";");
            }

            this.Indent--;
            this.Output.WriteLine();
            this.Indent++;
        }

        // Generates code for the specified CodeDom based method invoke expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
            this.GenerateMethodReferenceExpression(e.Method);
            this.Output.Write("(");
            var first = true;
            foreach (CodeExpression parameter in e.Parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    this.Output.Write(", ");
                }

                if (parameter is CodeLambdaExpression)
                {
                    this.NormalizeExpression(parameter as CodeLambdaExpression);
                }
                else
                {
                    this.OutputExpressionList(new CodeExpressionCollection(new[] { parameter }));
                }
            }

            this.Output.Write(")");
        }

        protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                if (e.TargetObject is CodeBinaryOperatorExpression)
                {
                    this.Output.Write("(");
                    this.GenerateExpression(e.TargetObject);
                    this.Output.Write(")");
                }
                else
                {
                    this.GenerateExpression(e.TargetObject);
                }

                this.Output.Write(".");
            }

            this.OutputIdentifier(e.MethodName);

            if (e.TypeArguments.Count <= 0)
            {
                return;
            }

            var s = new StringBuilder();
            s.Append("<");
            bool first = true;
            foreach (CodeTypeReference t in e.TypeArguments)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    s.Append(", ");
                }

                s.Append(this.GetTypeOutput(t));
            }

            s.Append(">");
            this.Output.Write(s.ToString());
        }

        // Generates code for the specified CodeDom based method return statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
        {
            this.Output.Write("return");
            if (e.Expression != null)
            {
                this.Output.Write(" ");
                this.GenerateExpression(e.Expression);
            }

            this.Output.WriteLine(";");
        }

        protected override void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
        {
            this.OutputType(e.Type);
            this.Output.Write(".Default()");
        }

        // Generates code for the specified CodeDom based namespace representation.
        // 'e' indicates the namespace to generate code for.
        protected override void GenerateNamespace(CodeNamespace e)
        {
            this.Output.WriteLine(string.Empty);

            this.GenerateNamespaceImports(e);
            this.Output.WriteLine(string.Empty);

            this.GenerateCommentStatements(e.Comments);
            this.GenerateNamespaceStart(e);
            this.GenerateTypes(e);
            this.GenerateNamespaceEnd(e);
        }

        // Generates code for the specified CodeDom based namespace end representation.
        // 'e' indicates the namespace end to generate code for.
        protected override void GenerateNamespaceEnd(CodeNamespace e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                this.Indent--;
                this.Output.WriteLine("}");
            }

            if (this.mainClassName != null)
            {
                if (e.Name != null)
                {
                    this.OutputIdentifier(e.Name);
                    this.Output.Write(".");
                }

                this.OutputIdentifier(this.mainClassName);
                this.Output.Write(".");
                this.OutputIdentifier(this.mainMethodName);
                this.Output.WriteLine("();");
                this.mainClassName = null;
            }
        }

        // Generates code for the specified CodeDom based namespace import representation.
        // 'e' indicates the namespace import to generate code for.
        protected override void GenerateNamespaceImport(CodeNamespaceImport e)
        {
            this.OutputIdentifier(e.Namespace);
            this.Output.WriteLine(string.Empty);
        }

        // Generates code for the specified CodeDom based namespace start representation.
        // 'e' indicates the namespace start to generate code for.
        protected override void GenerateNamespaceStart(CodeNamespace e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                this.Output.Write("export module ");
                this.OutputIdentifier(e.Name);
                this.OutputStartingBrace();
                this.Indent++;
            }
        }

        // Generates code for the specified CodeDom based object creation expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
            this.Output.Write("new ");

            this.OutputType(e.CreateType);
            this.Output.Write("(");
            if (e.Parameters.Count == 1 && e.Parameters[0] is CodeLambdaExpression)
            {
                this.NormalizeExpression(e.Parameters[0] as CodeLambdaExpression);
            }
            else
            {
                this.OutputExpressionList(e.Parameters);
            }

            this.Output.Write(")");
        }

        protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                CodeAttributeDeclaration attr = e.CustomAttributes[0];
                if (attr.Name == "ParamArrayAttribute")
                {
                    this.Output.Write("... ");
                }
                else
                {
                    throw new Exception("No parameter attributes");
                }
            }

            this.OutputDirection(e.Direction);
            var name = e.Name;
            if (e.CustomAttributes.ToArrayExceptNull(x => x).Any(x => x.Name == "System.Runtime.InteropServices.OptionalAttribute"))
            {
                name = name + "?";
            }

            this.OutputTypeNamePair(e.Type, name);
        }

        // Generates code for the specified CodeDom based member property representation.
        // 'e' indicates the member property to generate code for.
        protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
        {
            if (!(this.IsCurrentClass || this.IsCurrentStruct || this.IsCurrentInterface))
            {
                return;
            }

            if (e.PrivateImplementationType != null)
            {
                return;
            }

            if (e.HasGet)
            {
                if (!this.IsCurrentInterface)
                {
                    if (e.PrivateImplementationType == null)
                    {
                        this.OutputMemberAccessModifier(e.Attributes);
                        this.OutputMemberScopeModifier(e.Attributes);
                    }
                }

                if (e.CustomAttributes.Count > 0)
                {
                    if (this.IsCurrentInterface)
                    {
                        this.Output.Write("public ");
                    }

                    this.OutputAttributeDeclarations(e.CustomAttributes);
                }

                if (this.IsCurrentInterface)
                {
                    this.OutputIdentifier(e.Name);
                    if (e.Type.BaseType == "System.Nullable`1")
                    {
                        this.Output.Write("?");
                    }

                    this.Output.Write(" : ");
                    this.OutputType(e.Type);
                    this.Output.WriteLine(";");

                    return;
                }

                this.Output.Write("get ");
                if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
                {
                    this.Output.Write(e.PrivateImplementationType.BaseType);
                    this.Output.Write(".");
                }

                this.OutputIdentifier(e.Name);
                if (e.Parameters.Count > 0)
                {
                    throw new Exception("No indexer declarations");
                }

                this.Output.Write("() : ");
                this.OutputType(e.Type);
                if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
                {
                    this.Output.WriteLine(";");
                }
                else
                {
                    this.OutputStartingBrace();
                    this.Indent++;
                    this.GenerateStatements(e.GetStatements);
                    this.Indent--;
                    this.Output.WriteLine("}");
                }

                this.Indent--;
                this.Output.WriteLine(string.Empty);
                this.Indent++;
            }

            if (e.HasSet)
            {
                if (!this.IsCurrentInterface)
                {
                    if (e.PrivateImplementationType == null)
                    {
                        this.OutputMemberAccessModifier(e.Attributes);
                        this.OutputMemberScopeModifier(e.Attributes);
                    }
                }

                if (e.CustomAttributes.Count > 0 && !e.HasGet)
                {
                    if (this.IsCurrentInterface)
                    {
                        this.Output.Write("public ");
                    }

                    this.OutputAttributeDeclarations(e.CustomAttributes);
                    this.Output.WriteLine(string.Empty);
                }

                this.Output.Write(" set ");
                if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
                {
                    this.Output.Write(e.PrivateImplementationType.BaseType);
                    this.Output.Write(".");
                }

                this.OutputIdentifier(e.Name);
                this.Output.Write("(");
                this.OutputTypeNamePair(e.Type, "value");
                if (e.Parameters.Count > 0)
                {
                    throw new Exception("No indexer declarations");
                }

                this.Output.Write(")");
                if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
                {
                    this.Output.WriteLine(";");
                }
                else
                {
                    this.OutputStartingBrace();
                    this.Indent++;
                    this.GenerateStatements(e.SetStatements);
                    this.Indent--;
                    this.Output.WriteLine("}");
                    this.Indent--;
                    this.Output.WriteLine();
                    this.Indent++;
                }
            }
        }

        // Generates code for the specified CodeDom based property reference expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                if (e.PropertyName == "Nothing")
                {
                    this.Output.Write("new ");
                }

                this.GenerateExpression(e.TargetObject);
                if (e.PropertyName == "Nothing")
                {
                    this.Output.Write("()");
                }

                this.Output.Write(".");
            }

            this.OutputIdentifier(e.PropertyName);
        }

        protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
        {
            this.Output.Write("value");
        }

        protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
        {
            this.GenerateExpression(e.Event.TargetObject);
            this.Output.Write(".remove_");
            this.Output.Write(e.Event.EventName);
            this.Output.Write("(");
            this.GenerateExpression(e.Listener);
            this.Output.WriteLine(");");
        }

        protected override void GenerateSingleFloatValue(float s)
        {
            this.Output.Write("float(");
            this.Output.Write(s.ToString(CultureInfo.InvariantCulture));
            this.Output.Write(")");
        }

        // Generates code for the specified CodeDom based snippet expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateSnippetExpression(CodeSnippetExpression e)
        {
            this.Output.Write(e.Value);
        }

        // Generates code for the specified CodeDom based snippet class member representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
        {
            this.Output.Write(e.Text);
        }

        protected override void GenerateSnippetStatement(CodeSnippetStatement e)
        {
            this.Output.WriteLine(e.Value);
        }

        // Generates code for the specified CodeDom based this reference expression representation.
        // 'e' indicates the expression to generate code for.
        protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
        {
            this.Output.Write("this");
        }

        // Generates code for the specified CodeDom based throw exception statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
            this.Output.Write("throw");
            if (e.ToThrow != null)
            {
                this.Output.Write(" ");
                this.GenerateExpression(e.ToThrow);
            }

            this.Output.WriteLine(";");
        }

        protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
        {
            this.Output.Write("try");
            this.OutputStartingBrace();
            this.Indent++;
            this.GenerateStatements(e.TryStatements);
            this.Indent--;
            CodeCatchClauseCollection catches = e.CatchClauses;
            if (catches.Count > 0)
            {
                IEnumerator en = catches.GetEnumerator();
                while (en.MoveNext())
                {
                    this.Output.Write("}");
                    if (this.Options.ElseOnClosing)
                    {
                        this.Output.Write(" ");
                    }
                    else
                    {
                        this.Output.WriteLine(string.Empty);
                    }

                    var current = (CodeCatchClause)en.Current;
                    this.Output.Write("catch (");
                    this.OutputIdentifier(current.LocalName);
                    this.Output.Write(": ");
                    this.OutputType(current.CatchExceptionType);
                    this.Output.Write(")");
                    this.OutputStartingBrace();
                    this.Indent++;
                    this.GenerateStatements(current.Statements);
                    this.Indent--;
                }
            }

            CodeStatementCollection finallyStatements = e.FinallyStatements;
            if (finallyStatements.Count > 0)
            {
                this.Output.Write("}");
                if (this.Options.ElseOnClosing)
                {
                    this.Output.Write(" ");
                }
                else
                {
                    this.Output.WriteLine(string.Empty);
                }

                this.Output.Write("finally");
                this.OutputStartingBrace();
                this.Indent++;
                this.GenerateStatements(finallyStatements);
                this.Indent--;
            }

            this.Output.WriteLine("}");
        }

        // Generates code for the specified CodeDom based class constructor representation.
        // 'e' indicates the constructor to generate code for.
        protected override void GenerateTypeConstructor(CodeTypeConstructor e)
        {
            if (!(this.IsCurrentClass || this.IsCurrentStruct))
            {
                return;
            }

            this.Output.Write("static ");
            this.OutputIdentifier(this.CurrentTypeName);
            this.OutputStartingBrace();
            this.Indent++;
            this.GenerateStatements(e.Statements);
            this.Indent--;
            this.Output.WriteLine("}");
        }

        // Generates code for the specified CodeDom based class start representation.
        // 'e' indicates the start of the class.
        protected override void GenerateTypeStart(CodeTypeDeclaration e)
        {
            if (string.IsNullOrEmpty(e.Name))
            {
                return;
            }

            if (this.IsCurrentDelegate)
            {
                throw new Exception("No delegate declarations");
            }

            this.OutputTypeVisibility(e.TypeAttributes);

            this.OutputTypeAttributes(e.TypeAttributes, this.IsCurrentStruct, this.IsCurrentEnum);
            this.OutputIdentifier(e.Name);


            if (this.IsCurrentEnum)
            {
                if (e.BaseTypes.Count > 1)
                {
                    throw new Exception("Too many base types");
                }
            }
            else
            {
                bool first = true;
                bool second = false;
                foreach (CodeTypeReference typeRef in e.BaseTypes)
                {
                    bool isInterface = typeRef.BaseType.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last()
                        .Pipe(s => s.StartsWith("I") && char.IsUpper(s.Skip(1).First()))
                        && (typeRef.UserData["DomainType"] as Type)?.IsClass != true;

                    if (isInterface && first)
                    {
                        first = false;
                        second = true;
                    }

                    if (first)
                    {
                        this.Output.Write(" extends ");
                        first = false;
                        second = true;
                    }
                    else if (second)
                    {
                        this.Output.Write(this.IsCurrentInterface ? " extends " : " implements ");

                        second = false;
                    }
                    else
                    {
                        this.Output.Write(", ");
                    }

                    this.OutputType(typeRef);
                }
            }

            this.OutputStartingBrace();
            this.Indent++;
        }

        protected override void GenerateTypeEnd(CodeTypeDeclaration e)
        {
            if (!this.IsCurrentDelegate && !string.IsNullOrEmpty(e.Name))
            {
                this.Indent--;
                this.Output.WriteLine("}");
                this.Output.WriteLine();
            }
        }

        protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
        {
            this.OutputType(e.Type);
        }

        protected override string GetTypeOutput(CodeTypeReference typeRef)
        {
            StringBuilder builder;
            if (typeRef.ArrayElementType != null)
            {
                // Recurse up
                builder = new StringBuilder(this.GetTypeOutput(typeRef.ArrayElementType));
            }
            else
            {
                if (typeRef.BaseType == "System.Nullable`1")
                {
                    builder = new StringBuilder(this.GetTypeOutput(typeRef.TypeArguments[0]));
                }
                else if (typeRef.BaseType == "System.Array`1" || typeRef.BaseType == "KnockoutObservableArray`1")
                {
                    var ct = typeRef.TypeArguments[0];
                    var f = this.GetTypeOutput(ct);

                    builder = new StringBuilder(typeRef.BaseType == "System.Array`1" ? $"Array<{f}>" : $"KnockoutObservableArray<{f}>");
                }
                else
                {
                    builder = new StringBuilder(this.GetBaseTypeOutput(typeRef.BaseType));
                    if (typeRef.TypeArguments.Count > 0)
                    {
                        var stringType = builder.ToString();
                        var wrongSymbolPosition = stringType.Contains("`")
                            ? stringType.IndexOf("`", StringComparison.InvariantCulture)
                            : stringType.Length - 2;
                        builder = new StringBuilder(stringType.Substring(0, wrongSymbolPosition));
                        builder.Append("<");
                        bool first = true;
                        foreach (CodeTypeReference t in typeRef.TypeArguments)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                builder.Append(", ");
                            }

                            builder.Append(this.GetTypeOutput(t));
                        }

                        builder.Append(">");
                    }
                }
            }

            // Now spit out the array postfix
            if (typeRef.ArrayRank > 0)
            {
                char[] results = new char[typeRef.ArrayRank + 1];
                results[0] = '[';
                results[typeRef.ArrayRank] = ']';
                for (int i = 1; i < typeRef.ArrayRank; i++)
                {
                    results[i] = ',';
                }

                builder.Append(new string(results));
            }

            return builder.ToString();
        }

        // Generates code for the specified CodeDom based variable declaration statement representation.
        // 'e' indicates the statement to generate code for.
        protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
            if (e.Type.BaseType == "any")
            {
                this.Output.Write("let " + e.Name + ":" + e.Type.BaseType);
            }
            else
            {
                this.Output.Write("let " + e.Name);
            }

            if (e.InitExpression != null)
            {
                this.Output.Write(" = ");
                this.GenerateExpression(e.InitExpression);
            }

            this.Output.WriteLine(";");
        }

        protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
            this.OutputIdentifier(e.VariableName);
        }

        // Gets whether the specified value is a valid identifier.
        // 'value' is the string to test for validity as an identifier.
        protected override bool IsValidIdentifier(string value)
        {
            // identifiers must be 1 char or longer
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return true;
        }

        // Gets the token used to represent 'null'.

        // Generates code for the specified System.CodeDom.CodeAttributeBlock.
        protected override void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
        {
            return;
            if (attributes.Count == 0)
            {
                return;
            }

            this.Output.Write("// ");
            this.GenerateAttributeDeclarationsStart(attributes);
            IEnumerator en = attributes.GetEnumerator();
            while (en.MoveNext())
            {
                var current = (CodeAttributeDeclaration)en.Current;
                this.Output.Write(this.GetBaseTypeOutput(current.Name));
                this.Output.Write("(");

                bool firstArg = true;
                foreach (CodeAttributeArgument arg in current.Arguments)
                {
                    if (firstArg)
                    {
                        firstArg = false;
                    }
                    else
                    {
                        this.Output.Write(", ");
                    }

                    this.OutputAttributeArgument(arg);
                }

                this.Output.Write(") ");
            }

            this.GenerateAttributeDeclarationsEnd(attributes);
        }

        protected override void OutputDirection(FieldDirection dir)
        {
            switch (dir)
            {
                case FieldDirection.In:
                    break;
                case FieldDirection.Out:
                case FieldDirection.Ref:
                    if (!this.isArgumentList)
                    {
                        throw new Exception("No parameter direction");
                    }
                    else
                    {
                        this.Output.Write("&");
                    }

                    break;
            }
        }

        protected override void OutputIdentifier(string ident)
        {
            this.Output.Write(this.CreateEscapedIdentifier(ident));
        }

        // Generates code for the specified CodeDom based member access modifier representation.
        // 'attributes' indicates the access modifier to generate code for.
        protected override void OutputMemberAccessModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.AccessMask)
            {
                case MemberAttributes.Assembly:
                case MemberAttributes.FamilyAndAssembly:
                case MemberAttributes.Family:
                case MemberAttributes.FamilyOrAssembly:
                case MemberAttributes.Public:
                    this.Output.Write("public ");
                    break;
                default:
                    this.Output.Write("private ");
                    break;
            }
        }

        // Generates code for the specified CodeDom based member scope modifier representation.
        // 'attributes' indicates the scope modifier to generate code for.</para>
        protected override void OutputMemberScopeModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.ScopeMask)
            {
                case MemberAttributes.Abstract:
                    this.Output.Write("abstract ");
                    break;
                case MemberAttributes.Final:
                    break;
                case MemberAttributes.Static:
                    this.Output.Write("static ");
                    break;
                case MemberAttributes.Override:
                    this.Output.Write("override ");
                    break;
            }
        }

        // Generates code for the specified CodeDom based parameter declaration expression representation.
        // 'parameters' indicates the parameter declaration expressions to generate code for.
        protected override void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
        {
            bool first = true;
            IEnumerator en = parameters.GetEnumerator();
            while (en.MoveNext())
            {
                var current = (CodeParameterDeclarationExpression)en.Current;
                if (first)
                {
                    first = false;
                }
                else
                {
                    this.Output.Write(", ");
                }

                this.GenerateExpression(current);
            }
        }

        // Generates code for the specified CodeDom based return type representation.
        // 'typeRef' indicates the return type to generate code for.
        protected override void OutputType(CodeTypeReference typeRef)
        {
            this.Output.Write(this.GetTypeOutput(typeRef));
        }

        protected override void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
        {
            if (isEnum)
            {
                this.Output.Write("enum ");
            }
            else
            {
                switch (attributes & TypeAttributes.ClassSemanticsMask)
                {
                    case TypeAttributes.Class:
                        this.Output.Write("class ");
                        break;
                    case TypeAttributes.Interface:
                        this.Output.Write("interface ");
                        break;
                }
            }
        }

        // Generates code for the specified CodeDom based type name pair representation.
        // 'typeRef' indicates the type to generate code for.
        // 'name' name to generate code for.
        // remarks: This is commonly used for variable declarations.
        protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name)
        {
            this.OutputIdentifier(name);
            this.Output.Write(": ");
            this.OutputType(typeRef);
        }

        protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
        {
            Match m = OutputReg.Match(line);
            if (m.Success)
            {
                var ce = new CompilerError();

                // The location is optional since the error can not always be traced to a file.
                if (m.Groups[1].Success)
                {
                    ce.FileName = m.Groups[2].Value;
                    ce.Line = int.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture);
                    ce.Column = int.Parse(m.Groups[5].Value, CultureInfo.InvariantCulture);
                }

                if (string.Compare(m.Groups[7].Value, "warning", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ce.IsWarning = true;
                }

                ce.ErrorNumber = m.Groups[8].Value;
                ce.ErrorText = m.Groups[9].Value;

                results.Errors.Add(ce);
            }
        }

        protected override string QuoteSnippetString(string value)
        {
            return this.QuoteSnippetStringCStyle(value);
        }

        protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
        {
            var expression = e as JsObjectCreateExpression;
            if (expression != null)
            {
                var initialData = expression.Properties.Select(x => $"{x.Key} : {(this.IsSimpleType(x.Value) ? "null" : this.GetBaseTypeOutput(x.Value))}").Join(", ");
                this.Output.Write($"{{{initialData}}}");
                return;
            }

            if (null == e.Value)
            {
                this.Output.Write("undefined");
            }
            else if (e.Value is DBNull)
            {
                this.Output.Write("null");
            }
            else if (e.Value is char)
            {
                this.GeneratePrimitiveChar((char)e.Value);
            }
            else
            {
                base.GeneratePrimitiveExpression(e);
            }
        }

        protected override bool Supports(GeneratorSupport support)
        {
            return (support & LanguageSupport) == support;
        }

        protected override void OutputOperator(CodeBinaryOperatorType op)
        {
            if (op == CodeBinaryOperatorType.ValueEquality || op == CodeBinaryOperatorType.IdentityEquality)
            {
                this.Output.Write("="); // tripple equals support
            }

            base.OutputOperator(op);
        }

        private bool IsKeyword(string value)
        {
            return Keywords.ContainsKey(value);
        }

        private void NormalizeExpression(CodeLambdaExpression lambdaExpression)
        {
            var parameters = lambdaExpression.Parameters
                .ToArrayExceptNull(v => v)
                .Join(
                ", ",
                exp =>
                {
                    if (exp.Type.BaseType == VoidType.BaseType)
                    {
                        return exp.Name;
                    }

                    var name = exp.Name;
                    var type = this.GetTypeOutput(exp.Type);
                    return $"{name}: {type}";
                });

            this.Output.Write(parameters.Length == 1 ? $"{parameters} => {{" : $"({parameters}) => {{");
            this.Indent++;
            this.Output.WriteLine();
            if (lambdaExpression.Statements.Count == 1)
            {
                this.Output.Write("return ");
            }

            this.GenerateStatements(lambdaExpression.Statements);
            this.Indent--;
            this.Output.Write("}");
        }

        private void GenerateAssemblyAttributes(CodeAttributeDeclarationCollection attributes)
        {
            if (attributes.Count == 0)
            {
                return;
            }

            IEnumerator en = attributes.GetEnumerator();
            while (en.MoveNext())
            {
                this.Output.Write("[");
                this.Output.Write("assembly: ");
                var current = (CodeAttributeDeclaration)en.Current;
                this.Output.Write(this.GetBaseTypeOutput(current.Name));
                this.Output.Write("(");
                bool firstArg = true;
                foreach (CodeAttributeArgument arg in current.Arguments)
                {
                    if (firstArg)
                    {
                        firstArg = false;
                    }
                    else
                    {
                        this.Output.Write(", ");
                    }

                    this.OutputAttributeArgument(arg);
                }

                this.Output.Write(")");
                this.Output.Write("]");
                this.Output.WriteLine();
            }
        }

        private bool IsSimpleType(string baseType)
        {
            if (baseType.Length == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Byte", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Int16", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Int32", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Int64", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.SByte", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.UInt16", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.UInt32", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.UInt64", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Decimal", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Single", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Double", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Boolean", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Char", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.DateTime", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Nullable`1", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.Guid", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            if (string.Compare(baseType, "System.String", StringComparison.Ordinal) == 0)
            {
                return true;
            }

            return string.Compare(baseType, "System.Void", StringComparison.Ordinal) == 0;
        }

        // Used to output the T part of new T[...]
        private string GetBaseTypeOutput(string baseType)
        {
            // replace + with . for nested classes.
            baseType = PropertyInfoExtensions.GetBasedOutputType(baseType).Replace('+', '.');
            return this.CreateEscapedIdentifier(baseType);
        }

        private void RenderLambdaExpressionBody(CodeStatement[] codeStatements)
        {
            if (codeStatements == null)
            {
                throw new ArgumentNullException(nameof(codeStatements));
            }

            if (codeStatements.Length == 1)
            {
                var returnExpr = (codeStatements[0] as CodeMethodReturnStatement)?.Expression;

                if (returnExpr != null)
                {
                    this.Output.Write("return");
                    this.Output.Write(" ");
                    this.GenerateExpression(returnExpr);
                }

                if (codeStatements[0] is CodeExpressionStatement)
                {
                    this.forLoopHack = true;
                    this.GenerateExpressionStatement(codeStatements[0] as CodeExpressionStatement);
                    this.forLoopHack = false;
                }
            }
        }

        private void OutputStartingBrace()
        {
            if (this.Options.BracingStyle == "C")
            {
                this.Output.WriteLine(string.Empty);
                this.Output.WriteLine("{");
            }
            else
            {
                this.Output.WriteLine(" {");
            }
        }

        private void OutputTypeVisibility(TypeAttributes attributes)
        {
            switch (attributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.NestedAssembly:
                case TypeAttributes.NestedFamANDAssem:
                case TypeAttributes.NestedFamily:
                case TypeAttributes.NestedFamORAssem:
                    this.Output.Write("static ");
                    break;
                case TypeAttributes.NotPublic:
                    this.Output.Write("private ");
                    break;
                case TypeAttributes.NestedPrivate:
                    this.Output.Write("private static ");
                    break;
                case TypeAttributes.NestedPublic:
                    this.Output.Write("public static ");
                    break;
                default:
                    this.Output.Write("export ");
                    break;
            }
        }

        private void GeneratePrimitiveChar(char c)
        {
            this.Output.Write('\'');
            switch (c)
            {
                case '\r':
                    this.Output.Write("\\r");
                    break;
                case '\t':
                    this.Output.Write("\\t");
                    break;
                case '\"':
                    this.Output.Write("\\\"");
                    break;
                case '\'':
                    this.Output.Write("\\\'");
                    break;
                case '\\':
                    this.Output.Write("\\\\");
                    break;
                case '\0':
                    this.Output.Write("\\0");
                    break;
                case '\n':
                    this.Output.Write("\\n");
                    break;
                case '\u2028':
                    this.Output.Write("\\u2028");
                    break;
                case '\u2029':
                    this.Output.Write("\\u2029");
                    break;
                default:
                    this.Output.Write(c);
                    break;
            }

            this.Output.Write('\'');
        }

        // Provides conversion to C-style formatting with escape codes.
        private string QuoteSnippetStringCStyle(string value)
        {
            char[] chars = value.ToCharArray();
            var b = new StringBuilder(value.Length + 5);

            b.Append("'"); //???

            int nextBreak = MaxLineLength;
            for (int i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '\r':
                        b.Append("\\r");
                        break;
                    case '\t':
                        b.Append("\\t");
                        break;
                    case '\"':
                        b.Append("\\\"");
                        break;
                    case '\'':
                        b.Append("\\\'");
                        break;
                    case '\\':
                        b.Append("\\\\");
                        break;
                    case '\0':
                        b.Append("\\0");
                        break;
                    case '\n':
                        b.Append("\\n");
                        break;
                    case '\u2028':
                        b.Append("\\u2028");
                        break;
                    case '\u2029':
                        b.Append("\\u2029");
                        break;
                    default:
                        b.Append(chars[i]);
                        break;
                }

                // Insert a newline but only if we've reached max line, there are more characters,
                // length, and we're not in the middle of a surrogate pair.
                if (i >= nextBreak && i + 1 < chars.Length &&
                    (!this.IsSurrogateStart(chars[i]) || !this.IsSurrogateEnd(chars[i + 1])))
                {
                    nextBreak = i + MaxLineLength;
                    b.Append($"' + {Environment.NewLine}'"); //???
                }
            }

            b.Append("'"); // ???

            return b.ToString();
        }

        private bool IsSurrogateStart(char c)
        {
            return 0xD800 <= c && c <= 0xDBFF; // Is code point the start (high surrogate) of a surrogate pair?
        }

        private bool IsSurrogateEnd(char c)
        {
            return 0xDC00 <= c && c <= 0xDFFF; // Is code point the end (i.e. low surrogate) of a surrogate pair?
        }
    }
}
